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
        _cat = cat;
        _price = price;
    }

    /// <summary>
    /// Фильтр по складу/городу, типу, диаметру, стенке, ГОСТ, марке.
    /// Если переданы Unit+Quantity и известен единственный StockId — вернём EffectivePricePerMeter.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListItemDto>>> Get([FromQuery] ProductFilterDto q, CancellationToken ct)
    {
        // 1) База: Номенклатура + фильтры
        var nm = _cat.Nomenclature.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.IDType))              nm = nm.Where(x => x.IDType == q.IDType);
        if (q.Diameter.HasValue)                               nm = nm.Where(x => x.Diameter == q.Diameter.Value);
        if (q.PipeWallThickness.HasValue)                      nm = nm.Where(x => x.PipeWallThickness == q.PipeWallThickness.Value);
        if (!string.IsNullOrWhiteSpace(q.Gost))                nm = nm.Where(x => x.Gost == q.Gost);
        if (!string.IsNullOrWhiteSpace(q.SteelGrade))          nm = nm.Where(x => x.SteelGrade == q.SteelGrade);

        // 2) Фильтр цен по складу
        var pricesQ = _cat.Prices.AsNoTracking().AsQueryable();   // <— было priceQ (не объявлено)
        List<string>? stockIdsFromCity = null;
        string? chosenStockId = null;

        if (!string.IsNullOrWhiteSpace(q.StockId))
        {
            pricesQ = pricesQ.Where(p => p.IDStock == q.StockId);
            chosenStockId = q.StockId;
        }
        else if (!string.IsNullOrWhiteSpace(q.StockCity))
        {
            stockIdsFromCity = await _cat.Stocks
                .Where(s => s.StockCity == q.StockCity)
                .Select(s => s.IDStock)
                .ToListAsync(ct);

            if (stockIdsFromCity.Count > 0)
                pricesQ = pricesQ.Where(p => stockIdsFromCity.Contains(p.IDStock));

            if (stockIdsFromCity.Count == 1)
                chosenStockId = stockIdsFromCity[0];
        }

        // 3) Join + пагинация
        var skip = Math.Max(q.Page - 1, 0) * q.PageSize;

        var slice = await (
            from n in nm
            join pr in pricesQ on n.ID equals pr.ID
            orderby n.Name
            select new { n, pr }
        ).Skip(skip).Take(q.PageSize).ToListAsync(ct);

        // 4) Собираем DTO и считаем эффект. цену если можем
        var items = new List<ProductListItemDto>(slice.Count);
        bool canCalcEffective = q.Unit.HasValue && q.Quantity.HasValue && !string.IsNullOrWhiteSpace(chosenStockId);

        foreach (var row in slice)
        {
            decimal? effective = null;

            if (canCalcEffective)
            {
                try
                {
                    effective = await _price.GetEffectivePricePerMeterAsync(
                        row.n.ID, chosenStockId!, q.Quantity!.Value, q.Unit!.Value, ct);
                }
                catch
                {
                    // проглатываем — отдадим только BasePricePerMeter
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

    /// <summary>Быстрый расчёт финальной цены за метр под количество/единицу/склад.</summary>
    [HttpGet("{id:int}/quote")]
    public async Task<ActionResult<ProductQuoteResponse>> Quote(
        int id,
        [FromQuery] string stockId,
        [FromQuery] QuantityUnit unit,
        [FromQuery] double quantity,
        CancellationToken ct)
    {
        var pricePerMeter = await _price.GetEffectivePricePerMeterAsync(id, stockId, quantity, unit, ct);
        return Ok(new ProductQuoteResponse(id, stockId, unit, quantity, pricePerMeter));
    }
}