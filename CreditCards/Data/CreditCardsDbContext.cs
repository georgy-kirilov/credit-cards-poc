using Microsoft.EntityFrameworkCore;

namespace CreditCards.Data;

public sealed class CreditCardsDbContext(DbContextOptions<CreditCardsDbContext> options) : DbContext(options)
{
    public DbSet<Card> Cards => Set<Card>();

    public Task<Card?> GetCard(int cardId) => Cards.FirstOrDefaultAsync(c => c.Id == cardId);
}
