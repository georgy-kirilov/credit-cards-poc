using CreditCards.Contracts;
using CreditCards.Data;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CreditCards.Features;

public sealed class CardStatusChangedConsumer(
    CreditCardsDbContext _dbContext,
    ILogger<CardStatusChangedConsumer> _logger) : IConsumer<CardStatusChanged>
{
    public async Task Consume(ConsumeContext<CardStatusChanged> context)
    {
        var card = await _dbContext.GetCard(context.Message.CardId);

        if (card is null)
        {
            // Log error
            return;
        }

        var oldCardStatus = card.Status;

        card.ChangeStatus(context.Message.Status);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("5. Status for card with ID '{CardId}' has been changed from {OldCardStatus} to {NewCardStatus}.",
            card.Id,
            oldCardStatus,
            card.Status);
    }
}
