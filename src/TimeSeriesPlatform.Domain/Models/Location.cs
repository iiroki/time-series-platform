namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record Location : LocationData
{
    public required string Id { get; init; }
}
