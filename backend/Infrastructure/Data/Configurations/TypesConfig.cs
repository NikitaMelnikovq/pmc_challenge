using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data.Configurations;

public sealed class TypesConfig : IEntityTypeConfiguration<Types>
{
    public void Configure(EntityTypeBuilder<Types> e)
    {
        e.ToTable("types");
        e.HasKey(x => x.IDType);
        e.Property(x => x.IDType).HasColumnName("idtype");
        e.Property(x => x.Type).HasColumnName("type");
        e.Property(x => x.IDParentType).HasColumnName("idparenttype");
    }
}