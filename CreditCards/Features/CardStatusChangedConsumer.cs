using CreditCards.Contracts;
using CreditCards.Data;
using MassTransit;

namespace CreditCards.Features;

public sealed class CardStatusChangedConsumer(CreditCardsDbContext _dbContext) : IConsumer<CardStatusChanged>
{
    public async Task Consume(ConsumeContext<CardStatusChanged> context)
    {
        var card = await _dbContext.GetCard(context.Message.CardId);

        if (card is null)
        {
            // Log error
            return;
        }

        card.ChangeStatus(context.Message.Status);

        await _dbContext.SaveChangesAsync();
    }
}
