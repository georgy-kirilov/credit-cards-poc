namespace CreditCards.Contracts;

public sealed record CardStatusChanged(int CardId, CardStatus Status);
