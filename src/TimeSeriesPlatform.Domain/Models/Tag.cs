namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record Tag : TagData
{
    public required string Id { get; init; }
}
