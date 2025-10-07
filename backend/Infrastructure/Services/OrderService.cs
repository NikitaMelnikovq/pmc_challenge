using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public interface IOrderService
{
    Task<Order> CreateAsync(Guid cartId, string firstName, string lastName, string inn, string phone, string email, CancellationToken ct);
}

public sealed class OrderService : IOrderService
{
    private readonly AppDbContext _app;
    private readonly CatalogDbContext _cat;

    public OrderService(AppDbContext app, CatalogDbContext cat) { _app = app; _cat = cat; }

    public async Task<Order> CreateAsync(Guid cartId, string firstName, string lastName, string inn, string phone, string email, CancellationToken ct)
    {
        var cart = await _app.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId, ct)
            ?? throw new InvalidOperationException("Cart not found");

        if (!cart.Items.Any()) throw new InvalidOperationException("Cart is empty");

        // подтянем номенклатуру для конверсий
        var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToArray();
        var noms = await _cat.Nomenclature.Where(n => productIds.Contains(n.ID)).ToDictionaryAsync(n => n.ID, ct);

        decimal total = 0m;
        foreach (var it in cart.Items)
        {
            if (!noms.TryGetValue(it.ProductId, out var nom))
                throw new InvalidOperationException($"Nomenclature {it.ProductId} not found");
            total += new CartService(_app, _cat, null!).LineTotal(it, nom); // маленький трюк: используем формулу
        }

        var order = new Order {
            CartId = cart.Id,
            Cart = cart,
            FirstName = firstName,
            LastName = lastName,
            INN = inn,
            Phone = phone,
            Email = email,
            Total = total
        };

        _app.Orders.Add(order);
        await _app.SaveChangesAsync(ct);
        return order;
    }
}