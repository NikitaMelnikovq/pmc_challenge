using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Core.Services;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public sealed class CartService : ICartService
{
    private readonly AppDbContext _db;
    private readonly IDiscountService _discount;
    private readonly IPriceService _price;

    public CartService(AppDbContext db, IDiscountService discount, IPriceService price)
    {
        _db = db; _discount = discount; _price = price;
    }

    public async Task<Cart> GetOrCreateAsync(Guid cartId, CancellationToken ct)
    {
        var cart = await _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId, ct);

        if (cart is null)
        {
            cart = new Cart { Id = cartId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync(ct);
        }
        return cart;
    }

    public async Task<Cart> AddItemAsync(Guid cartId, int productId, double qty, QuantityUnit unit, CancellationToken ct)
    {
        var cart = await GetOrCreateAsync(cartId, ct);
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.IsActive, ct)
            ?? throw new InvalidOperationException("Product not found");

        var pricePerMeter = await _price.GetCurrentPricePerMeterAsync(productId, ct)
            ?? throw new InvalidOperationException("No current price for product");

        var discountPercent = _discount.GetPercent(unit, qty);

        var item = new CartItem
        {
            CartId = cart.Id,
            ProductId = product.Id,
            Quantity = qty,
            Unit = unit,
            UnitPriceMeter = pricePerMeter,
            DiscountPercent = discountPercent
        };

        cart.Items.Add(item);
        await _db.SaveChangesAsync(ct);
        return cart;
    }

    public static decimal LineTotal(CartItem it)
    {
        // Считаем итог по позиции:
        // если в метрах — qty * pricePerMeter
        // если в тоннах — конвертируем тонны в метры: (тонн * 1000) / (кг/м)
        var meters = it.Unit == QuantityUnit.Meter
            ? it.Quantity
            : (it.Quantity * 1000.0) / it.Product.KgPerMeter;

        var gross = (decimal)meters * it.UnitPriceMeter;
        var discount = gross * (decimal)(it.DiscountPercent / 100.0);
        return decimal.Round(gross - discount, 2);
    }
}