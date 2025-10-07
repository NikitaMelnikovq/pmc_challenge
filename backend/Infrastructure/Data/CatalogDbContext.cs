using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data;

public sealed class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> opt) : base(opt) { }

    public DbSet<Types>        Types        => Set<Types>();
    public DbSet<Nomenclature> Nomenclature => Set<Nomenclature>();
    public DbSet<Prices>       Prices       => Set<Prices>();
    public DbSet<Remnants>     Remnants     => Set<Remnants>();
    public DbSet<Stock>        Stocks       => Set<Stock>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.ApplyConfiguration(new Configurations.TypesConfig());
        b.ApplyConfiguration(new Configurations.NomenclatureConfig());
        b.ApplyConfiguration(new Configurations.PricesConfig());
        b.ApplyConfiguration(new Configurations.RemnantsConfig());
        b.ApplyConfiguration(new Configurations.StockConfig());
    }
}