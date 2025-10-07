using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data.Configurations;

public sealed class StockConfig : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> e)
    {
        e.ToTable("stock");
        e.HasKey(x => x.IDStock);
        e.Property(x => x.IDStock).HasColumnName("idstock");
        e.Property(x => x.StockCity).HasColumnName("stock");
        e.Property(x => x.StockName).HasColumnName("stockname");
    }
}
