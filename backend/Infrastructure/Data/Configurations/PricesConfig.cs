using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data.Configurations;

public sealed class PricesConfig : IEntityTypeConfiguration<Prices>
{
    public void Configure(EntityTypeBuilder<Prices> e)
    {
        e.ToTable("prices");
        e.HasKey(x => new { x.ID, x.IDStock });
        e.Property(x => x.ID).HasColumnName("id");
        e.Property(x => x.IDStock).HasColumnName("idstock");
        e.Property(x => x.PriceT).HasColumnName("pricet").HasColumnType("numeric(18,2)");
        e.Property(x => x.PriceLimitT1).HasColumnName("pricelimitt1");
        e.Property(x => x.PriceT1).HasColumnName("pricet1").HasColumnType("numeric(18,2)");
        e.Property(x => x.PriceLimitT2).HasColumnName("pricelimitt2");
        e.Property(x => x.PriceT2).HasColumnName("pricet2").HasColumnType("numeric(18,2)");
        e.Property(x => x.PriceM).HasColumnName("pricem").HasColumnType("numeric(18,2)");
        e.Property(x => x.PriceLimitM1).HasColumnName("pricelimitm1");
        e.Property(x => x.PriceM1).HasColumnName("pricem1").HasColumnType("numeric(18,2)");
        e.Property(x => x.PriceLimitM2).HasColumnName("pricelimitm2");
        e.Property(x => x.PriceM2).HasColumnName("pricem2").HasColumnType("numeric(18,2)");
        e.Property(x => x.NDS).HasColumnName("nds");
    }
}