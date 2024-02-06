namespace EpsAdapter.Data.Models;

public sealed class EpsCard
{
    public required Guid Id { get; init; }

    public required int SmartCheckCardId { get; init; }

    private EpsCard() { }

    public static EpsCard Create(Guid id, int smartCheckCardId) => new()
    {
        Id = id,
        SmartCheckCardId = smartCheckCardId
    };
}
