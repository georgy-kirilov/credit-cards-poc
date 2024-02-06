using CreditCards.Contracts;
using CreditCards.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreditCards.Features;

public static class RequestCardStatusChangeEndpoint
{
    public static async Task<IResult> Map(RequestCardStatusChangeRequest request, RequestCardStatusChangeHandler handler)
    {
        var result = await handler.Handle(request);

        if (result is not null)
        {
            return Results.BadRequest(result);
        }

        return Results.Ok();
    }
}

public sealed record RequestCardStatusChangeRequest(int CardId, CardStatus NewStatus);

public sealed class RequestCardStatusChangeHandler(
    CreditCardsDbContext _dbContext,
    IServiceProvider _serviceProvider,
    ILogger<RequestCardStatusChangeHandler> _logger)
{
    public async Task<string?> Handle(RequestCardStatusChangeRequest request)
    {
        Card? card = await _dbContext.GetCard(request.CardId);

        if (card is null)
        {
            return "Card not found.";
        }

        bool statusChangeRequested = card.RequestStatusChange(request.NewStatus);

        if (!statusChangeRequested)
        {
            return "Domain validation failed.";
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("1. A status change for card with ID '{CardId}' has been requested.", card.Id);

        // TODO: If we decide to use an outbox table, we would need to publish the message before calling SaveChangesAsync.

        var publisher = _serviceProvider.GetRequiredKeyedService<ICardOperationPublisher>(card.Issuer);

        await publisher.RequestCardStatusChange(request.CardId, request.NewStatus);

        return null;
    }
}
