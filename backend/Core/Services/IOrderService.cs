namespace SteelShop.Core.Services;

public interface IOrderService
{
    Task<SteelShop.Core.Entities.Order> CreateAsync(
        System.Guid cartId, string name, string phone, string email, string company,
        System.Threading.CancellationToken ct);
}