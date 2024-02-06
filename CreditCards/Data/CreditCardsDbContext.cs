using Microsoft.EntityFrameworkCore;

namespace CreditCards.Data;

public sealed class CreditCardsDbContext(DbContextOptions<CreditCardsDbContext> options) : DbContext(options)
{
    public const string Schema = "smartcheck";

    public DbSet<Card> Cards => Set<Card>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }

    public Task<Card?> GetCard(int cardId) => Cards.FirstOrDefaultAsync(c => c.Id == cardId);
}
