using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data.Configurations;

public sealed class NomenclatureConfig : IEntityTypeConfiguration<Nomenclature>
{
    public void Configure(EntityTypeBuilder<Nomenclature> e)
    {
        e.ToTable("nomenclature");
        e.HasKey(x => x.ID);
        e.Property(x => x.ID).HasColumnName("id");
        e.Property(x => x.IDCat).HasColumnName("idcat");
        e.Property(x => x.IDType).HasColumnName("idtype");
        e.Property(x => x.IDTypeNew).HasColumnName("idtypenew");
        e.Property(x => x.ProductionType).HasColumnName("productiontype");
        e.Property(x => x.IDFunctionType).HasColumnName("idfunctiontype");
        e.Property(x => x.Name).HasColumnName("name");
        e.Property(x => x.Gost).HasColumnName("gost");
        e.Property(x => x.FormOfLength).HasColumnName("formoflength");
        e.Property(x => x.Manufacturer).HasColumnName("manufacturer");
        e.Property(x => x.SteelGrade).HasColumnName("steelgrade");
        e.Property(x => x.Diameter).HasColumnName("diameter");
        e.Property(x => x.ProfileSize2).HasColumnName("profilesize2");
        e.Property(x => x.PipeWallThickness).HasColumnName("pipewallthickness");
        e.Property(x => x.Status).HasColumnName("status");
        e.Property(x => x.Koef).HasColumnName("koef");
        e.HasIndex(x => new { x.IDType, x.Diameter, x.PipeWallThickness, x.Gost, x.SteelGrade });
    }
}