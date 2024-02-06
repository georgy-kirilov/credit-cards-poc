using Microsoft.EntityFrameworkCore;
using EpsAdapter.Data.Models;

namespace EpsAdapter.Data;

public sealed class EpsAdapterDbContext(DbContextOptions<EpsAdapterDbContext> options) : DbContext(options)
{
    public const string Schema = "eps";

    public DbSet<EpsCard> Cards => Set<EpsCard>();

    public DbSet<EpsCardRequest> CardRequests => Set<EpsCardRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }

    public Task<EpsCard?> GetCard(int smartCheckCardId) => Cards.FirstOrDefaultAsync(c => c.SmartCheckCardId == smartCheckCardId);
}
