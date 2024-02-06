using CreditCards.Contracts;

namespace CreditCards.Data;

public sealed class Card
{
    public int Id { get; private set; }

    public required CardIssuer Issuer { get; init; }

    public CardStatus Status { get; private set; } = CardStatus.Requested;

    public CardStatus? RequestedStatus { get; private set; } = null;

    private Card() { }

    public bool RequestStatusChange(CardStatus newStatus)
    {
        if (RequestedStatus is null && Status == newStatus)
        {
            return false;
        }

        if (RequestedStatus == newStatus)
        {
            return false;
        }

        if (Status == CardStatus.Destroyed)
        {
            return false;
        }

        // More domain logic and validation

        RequestedStatus = newStatus;

        return true;
    }

    public void ChangeStatus(CardStatus newStatus)
    {
        // More domain logic and validation

        if (RequestedStatus == newStatus)
        {
            RequestedStatus = null;
        }

        Status = newStatus;
    }

    public static Card Create(CardIssuer cardIssuer) => new()
    {
        Issuer = cardIssuer
    };
}
