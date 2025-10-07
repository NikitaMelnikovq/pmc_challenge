namespace SteelShop.Core.Entities;

public sealed class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}