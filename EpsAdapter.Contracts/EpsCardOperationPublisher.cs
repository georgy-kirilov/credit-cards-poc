using CreditCards.Contracts;

namespace EpsAdapter.Contracts;

public sealed class EpsCardOperationPublisher(IPublishEndpoint _endpoint) : ICardOperationPublisher
{
    public async Task RequestCardStatusChange(int cardId, CardStatus newStatus)
    {
        await _endpoint.Publish(new RequestCardStatusChangeToEps(cardId, newStatus));
    }
}
