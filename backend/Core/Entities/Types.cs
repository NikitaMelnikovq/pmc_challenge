namespace SteelShop.Core.Entities;

public sealed class Types
{
    public int IDType { get; set; }
    public string Type { get; set; } = "";
    public int? IDParentType { get; set; }
}