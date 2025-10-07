using Microsoft.EntityFrameworkCore;
using SteelShop.Core.Entities;
using SteelShop.Core.Services;
using SteelShop.Infrastructure.Data;

namespace SteelShop.Infrastructure.Services;

public sealed class PriceService : IPriceService
{
    private readonly AppDbContext _db;
    public PriceService(AppDbContext db) => _db = db;

    public async Task<decimal?> GetCurrentPricePerMeterAsync(int productId, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        return await _db.PriceSnapshots
            .Where(p => p.ProductId == productId && p.ValidFrom <= now && (p.ValidTo == null || p.ValidTo > now))
            .OrderByDescending(p => p.ValidFrom)
            .Select(p => (decimal?)p.PricePerMeter)
            .FirstOrDefaultAsync(ct);
    }
}