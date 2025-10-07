namespace SteelShop.Core.Services;

public interface IPriceService
{
    Task<decimal?> GetCurrentPricePerMeterAsync(int productId, System.Threading.CancellationToken ct);
}