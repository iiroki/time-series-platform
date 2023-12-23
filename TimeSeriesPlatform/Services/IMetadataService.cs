using Iiroki.TimeSeriesPlatform.Database.Entities;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    // INTEGRATIONS

    public Task<List<IntegrationEntity>> GetIntegrationsAsync(CancellationToken ct);

    public Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct);

    // TAGS

    public Task<List<TagEntity>> GetTagsAsync(CancellationToken ct);

    public Task<TagEntity> CreateTagAsync(string todo, CancellationToken ct);
}
