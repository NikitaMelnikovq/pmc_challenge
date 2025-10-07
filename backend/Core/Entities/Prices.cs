namespace SteelShop.Core.Entities;

public sealed class Prices
{
    public int ID { get; set; }
    public string IDStock { get; set; } = "";

    public decimal? PriceT { get; set; }
    public double?  PriceLimitT1 { get; set; }
    public decimal? PriceT1 { get; set; }
    public double?  PriceLimitT2 { get; set; }
    public decimal? PriceT2 { get; set; }

    public decimal? PriceM { get; set; }
    public double?  PriceLimitM1 { get; set; }
    public decimal? PriceM1 { get; set; }
    public double?  PriceLimitM2 { get; set; }
    public decimal? PriceM2 { get; set; }

    public double? NDS { get; set; }
}