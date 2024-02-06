using EpsAdapter.Contracts;
using EpsAdapter.Data;
using EpsAdapter.Data.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EpsAdapter;

public sealed class RequestCardStatusChangeToEpsConsumer(
    EpsAdapterDbContext _dbContext,
    TimeProvider _timeProvider,
    ILogger<RequestCardStatusChangeToEpsConsumer> _logger) : IConsumer<RequestCardStatusChangeToEps>
{
    public async Task Consume(ConsumeContext<RequestCardStatusChangeToEps> context)
    {
        var epsCard = await _dbContext.GetCard(context.Message.CardId);

        if (epsCard is null)
        {
            // Log error
            return;
        }

        var cardRequest = EpsCardRequest.Create(
            context.Message.CardId,
            epsCard.Id,
            new ChangeCardStatusCardRequestParameters(context.Message.NewStatus),
            _timeProvider);

        await _dbContext.CardRequests.AddAsync(cardRequest);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("2. EPS card request for status change of card with ID '{CardId}' has been created.", context.Message.CardId);
    }
}
