using CreditCards.Contracts;
using MassTransit;

namespace EpsAdapter;

public sealed class EpsCardOperationPublisher(IPublishEndpoint _endpoint) : ICardOperationPublisher
{
    public Task<bool> RequestCardStatusChange(int cardId, CardStatus newStatus)
    {
        //Store card request into database
        _endpoint.Publish(new RequestCardStatusChangeToEps(cardId, newStatus);
        return Task.FromResult(true);
    }
}

public sealed record RequestCardStatusChangeToEps(int CardId, CardStatus NewStatus);