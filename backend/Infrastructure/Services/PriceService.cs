using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public interface IPriceService
{
    /// <summary>Возвращает эффективную цену за метр с учётом ступени (по введённой единице и количеству).</summary>
    Task<decimal> GetEffectivePricePerMeterAsync(int productId, int stockId, double quantity, QuantityUnit unit, CancellationToken ct);
}

public sealed class PriceService : IPriceService
{
    private readonly CatalogDbContext _cat;
    public PriceService(CatalogDbContext cat) => _cat = cat;

    public async Task<decimal> GetEffectivePricePerMeterAsync(int productId, int stockId, double qty, QuantityUnit unit, CancellationToken ct)
    {
        var price = await _cat.Prices.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == productId && p.IDStock == stockId, ct)
            ?? throw new InvalidOperationException("No price row for product/stock");

        var nom = await _cat.Nomenclature.AsNoTracking()
            .FirstOrDefaultAsync(n => n.ID == productId, ct)
            ?? throw new InvalidOperationException("Nomenclature not found");

        decimal? basePrice, tier1, tier2;
        double? limit1, limit2;

        if (unit == QuantityUnit.Ton)
        {
            basePrice = price.PriceT; tier1 = price.PriceT1; tier2 = price.PriceT2;
            limit1 = price.PriceLimitT1; limit2 = price.PriceLimitT2;
        }
        else
        {
            basePrice = price.PriceM; tier1 = price.PriceM1; tier2 = price.PriceM2;
            limit1 = price.PriceLimitM1; limit2 = price.PriceLimitM2;
        }

        decimal perUnit = basePrice ?? throw new InvalidOperationException("Base price missing");

        if (limit2.HasValue && qty >= limit2.Value && tier2.HasValue) perUnit = tier2.Value;
        else if (limit1.HasValue && qty >= limit1.Value && tier1.HasValue) perUnit = tier1.Value;

        if (unit == QuantityUnit.Ton)
        {
            var koef = nom.Koef ?? throw new InvalidOperationException("Koef is required for ton pricing");
            var metersPerTon = 1.0 / koef;
            var perMeter = perUnit / (decimal)metersPerTon;
            return decimal.Round(perMeter, 4);
        }
        else
        {
            return decimal.Round(perUnit, 4);
        }
    }
}