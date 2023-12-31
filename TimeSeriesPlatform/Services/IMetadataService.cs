using Iiroki.TimeSeriesPlatform.Database.Entities;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    // INTEGRATIONS

    public Task<List<IntegrationEntity>> GetIntegrationsAsync(CancellationToken ct);

    public Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct);

    // public Task<IntegrationEntity> UpdateIntegrationAsync(long id, string name, CancellationToken ct);

    // TAGS

    public Task<List<TagEntity>> GetTagsAsync(CancellationToken ct);

    public Task<TagEntity> CreateTagAsync(string name, string slug, CancellationToken ct);
}
