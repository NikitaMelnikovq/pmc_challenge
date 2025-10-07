using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public interface IPriceService
{
    Task<decimal> GetEffectivePricePerMeterAsync(
        int productId, string stockId, double quantity, QuantityUnit unit, CancellationToken ct);
}

public sealed class PriceService : IPriceService
{
    private readonly CatalogDbContext _cat;
    public PriceService(CatalogDbContext cat) => _cat = cat;

    public async Task<decimal> GetEffectivePricePerMeterAsync(
        int productId, string stockId, double quantity, QuantityUnit unit, CancellationToken ct)
    {
        var pr = await _cat.Prices.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == productId && p.IDStock == stockId, ct)
            ?? throw new InvalidOperationException("No price row for product/stock");

        if (unit == QuantityUnit.Meter)
        {
            var price = pr.PriceM ?? 0m;
            if (pr.PriceLimitM2.HasValue && quantity >= pr.PriceLimitM2 && pr.PriceM2.HasValue) price = pr.PriceM2.Value;
            else if (pr.PriceLimitM1.HasValue && quantity >= pr.PriceLimitM1 && pr.PriceM1.HasValue) price = pr.PriceM1.Value;
            return price;
        }
        else // QuantityUnit.Ton
        {
            var nom = await _cat.Nomenclature.AsNoTracking().FirstAsync(n => n.ID == productId, ct);
            if (!(nom.Koef.HasValue && nom.Koef.Value > 0)) throw new InvalidOperationException("Koef required");

            var priceT = pr.PriceT ?? 0m;
            if (pr.PriceLimitT2.HasValue && quantity >= pr.PriceLimitT2 && pr.PriceT2.HasValue) priceT = pr.PriceT2.Value;
            else if (pr.PriceLimitT1.HasValue && quantity >= pr.PriceLimitT1 && pr.PriceT1.HasValue) priceT = pr.PriceT1.Value;

            // Koef = t/m → цена за метр = цена за тонну * t_per_meter
            return priceT * (decimal)nom.Koef.Value;
        }
    }
}
