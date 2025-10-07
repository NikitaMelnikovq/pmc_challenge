namespace SteelShop.Core.Entities;

public sealed class Order
{
    public long Id { get; set; }
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public string FirstName { get; set; } = "";
    public string LastName  { get; set; } = "";
    public string INN       { get; set; } = "";
    public string Phone     { get; set; } = "";
    public string Email     { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public decimal Total { get; set; }
}