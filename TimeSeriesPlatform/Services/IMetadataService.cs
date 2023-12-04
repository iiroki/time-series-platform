using Iiroki.TimeSeriesPlatform.Database.Entities;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    public Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct);
}
