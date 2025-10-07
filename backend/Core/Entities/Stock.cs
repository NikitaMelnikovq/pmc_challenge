namespace SteelShop.Core.Entities;

public sealed class Stock
{
    public string IDStock { get; set; } = "";  // string (GUID)
    public string StockCity { get; set; } = ""; // поле "Stock" из JSON
    public string? StockName { get; set; }
}