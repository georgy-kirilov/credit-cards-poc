using CreditCards.Contracts;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EpsAdapter.Data.Models;

public abstract record CardRequestParameters(
    [property: JsonIgnore] CardRequestOperation CardRequestOperation)
{
    public string ToJsonPayload() => JsonSerializer.Serialize(this, GetType());
}

public sealed record ChangeCardStatusCardRequestParameters(
    CardStatus NewStatus) : CardRequestParameters(CardRequestOperation.ChangeCardStatus);
