namespace SteelShop.Api.Config;

public sealed class DiscountOptions
{
    public List<PerItemTier> PerItemTiers { get; set; } = new();
    public double MaxPercentPerItem { get; set; } = 15.0;
    public sealed class PerItemTier
    {
        public string Unit { get; set; } = "Meter";
        public double MinQuantity { get; set; }
        public double Percent { get; set; }
    }
}
