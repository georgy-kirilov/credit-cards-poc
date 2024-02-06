using CreditCards.Contracts;
using EpsAdapter.Data;
using EpsAdapter.Data.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EpsAdapter;

public sealed class EpsWorker(
    IBus _bus,
    ILogger<EpsWorker> _logger,
    IServiceProvider _serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5_000, stoppingToken);

            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<EpsAdapterDbContext>();

                var cardRequests = await dbContext.CardRequests
                    .Where(c => c.Status == EpsCardRequestStatus.Pending)
                    .OrderBy(c => c.CreatedOnUtc)
                    .Take(30)
                    .ToListAsync(stoppingToken);

                if (cardRequests.Count == 0)
                {
                    _logger.LogInformation("No pending EPS card requests were found.");
                    continue;
                }

                foreach (var cardRequest in cardRequests)
                {
                    if (cardRequest.Operation == CardRequestOperation.ChangeCardStatus)
                    {
                        await ChangeCardStatus(cardRequest, dbContext);
                    }
                }

                _logger.LogInformation("A total of {ProcessedRequests} EPS card request(s) have been processed.", cardRequests.Count);
            }
            catch
            {
            }
        }
    }

    private async Task ChangeCardStatus(EpsCardRequest cardRequest, EpsAdapterDbContext dbContext)
    {
        var parameters = cardRequest.GetParameters<ChangeCardStatusCardRequestParameters>();

        await CallExternalEpsApiToChangeCardStatus(cardRequest.SmartCheckCardId, parameters);

        cardRequest.Complete();

        await dbContext.SaveChangesAsync();

        _logger.LogInformation("4. EPS card request has been marked as complete.");

        // TODO: If we decide to use an outbox table, we would need to publish the message before calling SaveChangesAsync.

        await _bus.Publish(new CardStatusChanged(cardRequest.SmartCheckCardId, parameters.NewStatus));
    }

    private async Task<bool> CallExternalEpsApiToChangeCardStatus(int cardId, ChangeCardStatusCardRequestParameters parameters)
    {
        // Simulate a call to the external EPS API
        _logger.LogInformation("3. Calling the external EPS API to change status for card with ID '{CardId}' to '{NewStatus}'.",
            cardId,
            parameters.NewStatus);

        await Task.Delay(300);

        return true;
    }
}
