using CreditCards.Contracts;
using EpsAdapter.Data;
using EpsAdapter.Data.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EpsAdapter;

public sealed class EpsWorker(
    IServiceProvider _serviceProvider,
    IBus _bus) : BackgroundService
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

                foreach (var cardRequest in cardRequests)
                {
                    if (cardRequest.Operation == CardRequestOperation.ChangeCardStatus)
                    {
                        await ChangeCardStatus(cardRequest, dbContext);
                    }
                }
            }
            catch { }
        }
    }

    private async Task ChangeCardStatus(EpsCardRequest cardRequest, EpsAdapterDbContext dbContext)
    {
        var parameters = cardRequest.GetParameters<ChangeCardStatusCardRequestParameters>();

        await CallExternalEpsApiToChangeCardStatus(parameters);

        cardRequest.Complete();

        await _bus.Publish(new CardStatusChanged(cardRequest.SmartCheckCardId, parameters.NewStatus));

        await dbContext.SaveChangesAsync();
    }

    private async Task<bool> CallExternalEpsApiToChangeCardStatus(ChangeCardStatusCardRequestParameters parameters)
    {
        // Simulate a call to the external EPS API
        await Task.Delay(300);
        return true;
    }
}
