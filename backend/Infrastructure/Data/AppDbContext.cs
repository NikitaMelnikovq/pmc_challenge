using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;

namespace SteelShop.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Cart>  Carts  => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Cart>(e =>
        {
            e.ToTable("carts");
            e.HasKey(x => x.Id);
            e.Property(x => x.CreatedAt);
        });

        b.Entity<CartItem>(e =>
        {
            e.ToTable("cart_items");
            e.HasKey(x => x.Id);
            e.Property(x => x.UnitPricePerMeter).HasColumnType("numeric(18,4)");
            e.HasOne(x => x.Cart).WithMany(c => c.Items).HasForeignKey(x => x.CartId);
        });

        b.Entity<Order>(e =>
        {
            e.ToTable("orders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Total).HasColumnType("numeric(18,2)");
            e.HasOne(x => x.Cart).WithMany().HasForeignKey(x => x.CartId);
        });
    }
}
