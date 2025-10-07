using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Core.Services;
using SteelShop.Infrastructure.Data;
using SteelShop.Infrastructure.Services;

namespace SteelShop.Infrastructure.Services;

public sealed class OrderService : IOrderService
{
    private readonly AppDbContext _db;
    public OrderService(AppDbContext db) => _db = db;

    public async Task<Order> CreateAsync(Guid cartId, string name, string phone, string email, string company, System.Threading.CancellationToken ct)
    {
        var cart = await _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId, ct)
            ?? throw new InvalidOperationException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        var total = cart.Items.Aggregate(0m, (acc, it) => acc + CartService.LineTotal(it));

        var order = new Order
        {
            CartId = cart.Id,
            Cart = cart,
            CustomerName = name,
            Phone = phone,
            Email = email,
            Company = company,
            Total = total
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }
}