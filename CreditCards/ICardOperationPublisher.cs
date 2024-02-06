namespace CreditCards.Contracts;

public interface ICardOperationPublisher
{
    Task RequestCardStatusChange(int cardId, CardStatus newStatus);
}
