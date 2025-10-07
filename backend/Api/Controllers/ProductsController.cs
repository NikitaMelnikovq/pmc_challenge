using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelShop.Api.Dtos;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;
using SteelShop.Infrastructure.Services;

namespace SteelShop.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CatalogDbContext _cat;
    private readonly IPriceService _price;

    public ProductsController(CatalogDbContext cat, IPriceService price)
    {
        _cat = cat; _price = price;
    }

    /// <summary>
    /// Фильтр по: склад (StockId или StockCity), тип/диаметр/стенка/ГОСТ/марка.
    /// Если переданы Unit+Quantity и известен StockId — вернём EffectivePricePerMeter.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListItemDto>>> Get([FromQuery] ProductFilterDto q, CancellationToken ct)
    {
        // 1) База: номенклатура с фильтрами
        var nomQ = _cat.Nomenclature.AsNoTracking().AsQueryable();

        if (q.IDType.HasValue)           nomQ = nomQ.Where(x => x.IDType == q.IDType.Value);
        if (q.Diameter.HasValue)         nomQ = nomQ.Where(x => x.Diameter == q.Diameter.Value);
        if (q.PipeWallThickness.HasValue)nomQ = nomQ.Where(x => x.PipeWallThickness == q.PipeWallThickness.Value);
        if (!string.IsNullOrWhiteSpace(q.Gost))       nomQ = nomQ.Where(x => x.Gost == q.Gost);
        if (!string.IsNullOrWhiteSpace(q.SteelGrade)) nomQ = nomQ.Where(x => x.SteelGrade == q.SteelGrade);

        // 2) Фильтр по складу
        var priceQ = _cat.Prices.AsNoTracking().AsQueryable();

        List<int>? stockIds = null;
        if (q.StockId.HasValue)
        {
            priceQ = priceQ.Where(p => p.IDStock == q.StockId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(q.StockCity))
        {
            stockIds = await _cat.Stocks
                .Where(s => s.StockCity == q.StockCity)
                .Select(s => s.IDStock)
                .ToListAsync(ct);
            priceQ = priceQ.Where(p => stockIds.Contains(p.IDStock));
        }

        // 3) Join и пейджинация
        var skip = (q.Page - 1) * q.PageSize;
        var baseQuery =
            from n in nomQ
            join pr in priceQ on n.ID equals pr.ID
            orderby n.Name
            select new { n, pr };

        var slice = await baseQuery.Skip(skip).Take(q.PageSize).ToListAsync(ct);

        // 4) Собираем DTO. По умолчанию BasePricePerMeter = PriceM.
        // Если переданы Unit+Quantity и известен однозначный StockId — считаем EffectivePricePerMeter.
        var items = new List<ProductListItemDto>(slice.Count);
        bool canCalcEffective = q.Unit.HasValue && q.Quantity.HasValue && (q.StockId.HasValue || (stockIds != null && stockIds.Count == 1));

        foreach (var row in slice)
        {
            decimal? effective = null;
            var stockId = row.pr.IDStock;

            if (canCalcEffective)
            {
                var unit = q.Unit!.Value;
                var qty  = q.Quantity!.Value;

                // если StockCity => возьмём единственный id из списка
                if (!q.StockId.HasValue && stockIds is { Count: 1 })
                    stockId = stockIds[0];

                try
                {
                    effective = await _price.GetEffectivePricePerMeterAsync(row.n.ID, stockId, qty, unit, ct);
                }
                catch
                {
                    // молча падаем в null — фронт покажет базовую
                }
            }

            items.Add(new ProductListItemDto(
                row.n.ID,
                row.n.Name,
                row.n.Gost,
                row.n.SteelGrade,
                row.n.Diameter,
                row.n.PipeWallThickness,
                row.n.IDType,
                row.pr.IDStock,
                row.pr.PriceM,
                effective
            ));
        }

        return Ok(items);
    }

    [HttpGet("{id:int}/quote")]
    public async Task<ActionResult<ProductQuoteResponse>> Quote(
        int id,
        [FromQuery] int stockId,
        [FromQuery] QuantityUnit unit,
        [FromQuery] double quantity,
        CancellationToken ct)
    {
        var pricePerMeter = await _price.GetEffectivePricePerMeterAsync(id, stockId, quantity, unit, ct);
        return Ok(new ProductQuoteResponse(id, stockId, unit, quantity, pricePerMeter));
    }
}