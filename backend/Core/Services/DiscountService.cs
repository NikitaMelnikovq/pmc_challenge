using Microsoft.Extensions.Options;
using SteelShop.Api.Config;
using SteelShop.Core.Entities;
using SteelShop.Core.Services;

namespace SteelShop.Infrastructure.Services;

public sealed class DiscountService : IDiscountService
{
    private readonly DiscountOptions _opt;
    public DiscountService(IOptions<DiscountOptions> opt) => _opt = opt.Value;

    public double GetPercent(QuantityUnit unit, double quantity)
    {
        var unitStr = unit == QuantityUnit.Meter ? "Meter" : "Ton";
        var tier = _opt.PerItemTiers
            .Where(t => t.Unit.Equals(unitStr, StringComparison.OrdinalIgnoreCase) && quantity >= t.MinQuantity)
            .OrderByDescending(t => t.MinQuantity)
            .FirstOrDefault();

        var p = tier?.Percent ?? 0.0;
        return Math.Min(p, _opt.MaxPercentPerItem);
    }
}