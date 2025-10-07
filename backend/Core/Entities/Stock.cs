namespace SteelShop.Core.Entities;

public sealed class Stock
{
    public int IDStock { get; set; }
    public string StockCity { get; set; } = "";  // “Stock” из ТЗ, город
    public string? StockName { get; set; }
}