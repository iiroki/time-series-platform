namespace Iiroki.TimeSeriesPlatform.Models;

public record Tag : TagData
{
    public required string Id { get; init; }
}
