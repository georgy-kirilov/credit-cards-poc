namespace EpsAdapter;

public sealed class CardRequest
{
    public int Id { get; private set; }

    public required CardRequestOperation Operation { get; init; }

    public required int SmartCheckCardId { get; init; }

    public required Guid EpsCardId { get; init; }

    public required string ParametersPayload { get; init; }

    private CardRequest() { }

    public static CardRequest Create<TParameters>(
        int smartCheckCardId,
        Guid epsCardId,
        TParameters parameters)
        where TParameters : CardRequestParameters => new()
        {
            Operation = parameters.CardRequestOperation,
            SmartCheckCardId = smartCheckCardId,
            EpsCardId = epsCardId,
            ParametersPayload = parameters.ToJsonPayload(),
        };
}
