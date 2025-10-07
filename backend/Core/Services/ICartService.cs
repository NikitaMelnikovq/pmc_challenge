using SteelShop.Core.Entities;

namespace SteelShop.Core.Services;

public interface ICartService
{
    Task<Cart> GetOrCreateAsync(Guid cartId, System.Threading.CancellationToken ct);
    Task<Cart> AddItemAsync(Guid cartId, int productId, double qty, QuantityUnit unit, System.Threading.CancellationToken ct);
    // при желании: UpdateItemAsync / RemoveItemAsync
}