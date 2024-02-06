using CreditCards.Contracts;

namespace EpsAdapter.Contracts;

public sealed record RequestCardStatusChangeToEps(int CardId, CardStatus NewStatus);
