namespace SteelShop.Core.Entities;

public sealed class CartItem
{
    public long Id { get; set; }
    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }           // → Nomenclature.ID
    public int StockId { get; set; }             // склад, где покупаем
    public double Quantity { get; set; }         // введённое пользователем значение
    public QuantityUnit Unit { get; set; }       // Meter | Ton

    // зафиксированная “эффективная” цена за метр на момент добавления
    public decimal UnitPricePerMeter { get; set; }
}