namespace SteelShop.Core.Entities;

public sealed class Remnants
{
    public int ID { get; set; }          // → Nomenclature.ID
    public int IDStock { get; set; }
    public double? InStockT { get; set; }
    public double? InStockM { get; set; }
    public double? SoonArriveT { get; set; }
    public double? SoonArriveM { get; set; }
    public double? ReservedT { get; set; }
    public double? ReservedM { get; set; }
    public bool? UnderTheOrder { get; set; }
    public double? AvgTubeLength { get; set; }
    public double? AvgTubeWeight { get; set; }
}