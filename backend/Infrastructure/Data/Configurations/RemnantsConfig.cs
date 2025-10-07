using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data.Configurations;

public sealed class RemnantsConfig : IEntityTypeConfiguration<Remnants>
{
    public void Configure(EntityTypeBuilder<Remnants> e)
    {
        e.ToTable("remnants");
        e.HasKey(x => new { x.ID, x.IDStock });
        e.Property(x => x.ID).HasColumnName("id");
        e.Property(x => x.IDStock).HasColumnName("idstock");
        e.Property(x => x.InStockT).HasColumnName("instockt");
        e.Property(x => x.InStockM).HasColumnName("instockm");
        e.Property(x => x.SoonArriveT).HasColumnName("soonarrivet");
        e.Property(x => x.SoonArriveM).HasColumnName("soonarrivem");
        e.Property(x => x.ReservedT).HasColumnName("reservedt");
        e.Property(x => x.ReservedM).HasColumnName("reservedm");
        e.Property(x => x.UnderTheOrder).HasColumnName("undertheorder");
        e.Property(x => x.AvgTubeLength).HasColumnName("avgtubelength");
        e.Property(x => x.AvgTubeWeight).HasColumnName("avgtubeweight");
    }
}