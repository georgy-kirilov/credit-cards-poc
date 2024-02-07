namespace CreditCards.Features.Commands;

public sealed record RequestCardStatus(int CardId, CardStatusChangedConsumer CardStatus);
