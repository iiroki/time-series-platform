namespace Iiroki.TimeSeriesPlatform.Models;

public record Location : LocationData
{
    public required long Id { get; init; }
}
