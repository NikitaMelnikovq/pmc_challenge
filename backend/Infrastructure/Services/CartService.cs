using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public interface ICartService
{
    Task<Cart> GetOrCreateAsync(Guid cartId, CancellationToken ct);
    Task<Cart> AddItemAsync(Guid cartId, int productId, int stockId, double qty, QuantityUnit unit, CancellationToken ct);
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
        if (cart is null) { cart = new Cart { Id = cartId }; _app.Carts.Add(cart); await _app.SaveChangesAsync(ct); }
        return cart;
    }

    public async Task<Cart> AddItemAsync(Guid cartId, int productId, int stockId, double qty, QuantityUnit unit, CancellationToken ct)
    {
        var cart = await GetOrCreateAsync(cartId, ct);

        // проверка наличия прайса
        var effPricePerMeter = await _price.GetEffectivePricePerMeterAsync(productId, stockId, qty, unit, ct);

        cart.Items.Add(new CartItem {
            CartId = cart.Id,
            ProductId = productId,
            StockId = stockId,
            Quantity = qty,
            Unit = unit,
            UnitPricePerMeter = effPricePerMeter
        });

        await _app.SaveChangesAsync(ct);
        return cart;
    }

    public decimal LineTotal(CartItem it, Nomenclature nom)
    {
        // если в тоннах — переведём в метры: meters = tons / Koef
        var meters = it.Unit == QuantityUnit.Meter
            ? it.Quantity
            : it.Quantity / (nom.Koef ?? throw new InvalidOperationException("Koef required"));

        var gross = (decimal)meters * it.UnitPricePerMeter;
        return decimal.Round(gross, 2);
    }
}