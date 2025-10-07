namespace SteelShop.Api.Config;

public sealed class PriceRefreshOptions
{
    public bool Enabled { get; set; } = true;
    public int PollSeconds { get; set; } = 900;
}