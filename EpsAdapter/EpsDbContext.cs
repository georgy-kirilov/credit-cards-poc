using Microsoft.EntityFrameworkCore;

namespace EpsAdapter;

public sealed class EpsDbContext(DbContextOptions<EpsDbContext> options) : DbContext(options)
{
    public DbSet<CardRequest> CardRequests => Set<CardRequest>();
}
