namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record Integration : IntegrationData
{
    public required string Id { get; init; }
}
