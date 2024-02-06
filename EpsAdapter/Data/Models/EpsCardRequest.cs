using CreditCards.Contracts;
using System.Text.Json;

namespace EpsAdapter.Data.Models;

public sealed class EpsCardRequest
{
    public int Id { get; private set; }

    public EpsCardRequestStatus Status { get; private set; } = EpsCardRequestStatus.Pending;

    public required DateTimeOffset CreatedOnUtc { get; init; }

    public required CardRequestOperation Operation { get; init; }

    public required int SmartCheckCardId { get; init; }

    public required string ParametersPayload { get; init; }

    public required Guid EpsCardId { get; init; }

    public EpsCard EpsCard { get; } = null!;

    private EpsCardRequest() { }

    public void Complete() => Status = EpsCardRequestStatus.Complete;

    public TParameters GetParameters<TParameters>()
        where TParameters : CardRequestParameters
    {
        return JsonSerializer.Deserialize<TParameters>(ParametersPayload)
            ?? throw new ArgumentNullException();
    }

    public static EpsCardRequest Create<TParameters>(
        int smartCheckCardId,
        Guid epsCardId,
        TParameters parameters,
        TimeProvider timeProvider)
        where TParameters : CardRequestParameters => new()
        {
            CreatedOnUtc = timeProvider.GetUtcNow(),
            Operation = parameters.CardRequestOperation,
            SmartCheckCardId = smartCheckCardId,
            EpsCardId = epsCardId,
            ParametersPayload = parameters.ToJsonPayload(),
        };
}
