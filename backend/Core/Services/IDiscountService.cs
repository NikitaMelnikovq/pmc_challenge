using SteelShop.Core.Entities;

namespace SteelShop.Core.Services;
public interface IDiscountService
{
    double GetPercent(QuantityUnit unit, double quantity);
}