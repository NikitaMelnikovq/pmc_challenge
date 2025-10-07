namespace SteelShop.Core.Entities;

public sealed class CartItem
{
    public long Id { get; set; }
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }           // → Nomenclature.ID
    public string StockId { get; set; } = "";    // ← string (GUID)
    public double Quantity { get; set; }
    public QuantityUnit Unit { get; set; }
    public decimal UnitPricePerMeter { get; set; }
}