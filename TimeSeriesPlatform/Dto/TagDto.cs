namespace Iiroki.TimeSeriesPlatform.Dto;

public record TagDto
{
    public required long Id { get; init; }

    public required string Name { get; init; }
}
