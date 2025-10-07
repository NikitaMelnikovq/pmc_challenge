namespace SteelShop.Core.Entities;

public sealed class Nomenclature
{
    public int ID { get; set; }
    public int IDCat { get; set; }
    public string IDType { get; set; } = "";
    public string? IDTypeNew { get; set; }
    public string? ProductionType { get; set; }
    public string? IDFunctionType { get; set; }
    public string Name { get; set; } = "";
    public string? Gost { get; set; }
    public string? FormOfLength { get; set; }
    public string? Manufacturer { get; set; }
    public string? SteelGrade { get; set; }
    public double? Diameter { get; set; }
    public string? ProfileSize2 { get; set; }
    public double? PipeWallThickness { get; set; }
    public bool Status { get; set; }
    public double? Koef { get; set; }
}