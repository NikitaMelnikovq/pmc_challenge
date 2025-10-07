using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;


public interface ICartService
{
    Task<Cart> GetOrCreateAsync(Guid cartId, CancellationToken ct);
    Task<Cart> AddItemAsync(Guid cartId, int productId, string stockId, double qty, QuantityUnit unit, CancellationToken ct);
    decimal LineTotal(CartItem it, Nomenclature nom);
}

public sealed class CartService : ICartService
{
    private readonly AppDbContext _app;
    private readonly CatalogDbContext _cat;
    private readonly IPriceService _price;

    public CartService(AppDbContext app, CatalogDbContext cat, IPriceService price)
    { _app = app; _cat = cat; _price = price; }

    public async Task<Cart> GetOrCreateAsync(Guid cartId, CancellationToken ct)
    {
        var cart = await _app.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId, ct);
        if (cart is null)
        {
            cart = new Cart { Id = cartId, CreatedAt = DateTimeOffset.UtcNow };
            _app.Carts.Add(cart);
            await _app.SaveChangesAsync(ct);
        }
        return cart;
    }

    public async Task<Cart> AddItemAsync(Guid cartId, int productId, string stockId, double qty, QuantityUnit unit, CancellationToken ct)
    {
        var cart = await GetOrCreateAsync(cartId, ct);
        var pricePerMeter = await _price.GetEffectivePricePerMeterAsync(productId, stockId, qty, unit, ct);

        var item = new CartItem
        {
            CartId = cart.Id,
            ProductId = productId,
            StockId = stockId,                 // string GUID
            Quantity = qty,
            Unit = unit,
            UnitPricePerMeter = pricePerMeter
        };
        _app.CartItems.Add(item);
        await _app.SaveChangesAsync(ct);

        // перезагрузим корзину с Items
        await _app.Entry(cart).Collection(c => c.Items).LoadAsync(ct);
        return cart;
    }

    public decimal LineTotal(CartItem it, Nomenclature nom)
    {
        var meters = it.Unit == QuantityUnit.Meter
            ? it.Quantity
            : it.Quantity / (nom.Koef ?? throw new InvalidOperationException("Koef required"));

        return decimal.Round((decimal)meters * it.UnitPricePerMeter, 2);
    }
}