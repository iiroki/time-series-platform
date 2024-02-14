namespace Iiroki.TimeSeriesPlatform.Core.Models;

public record Tag : TagData
{
    public required string Id { get; init; }
}
