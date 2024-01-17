using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    // INTEGRATIONS

    public Task<List<IntegrationEntity>> GetIntegrationsAsync(CancellationToken ct = default);

    public Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct = default);

    // public Task<IntegrationEntity> UpdateIntegrationAsync(long id, string name, CancellationToken ct);

    // TAGS

    public Task<List<TagEntity>> GetTagsAsync(CancellationToken ct = default);

    public Task<TagEntity> CreateTagAsync(string name, string slug, CancellationToken ct = default);

    // LOCATIONS

    public Task<List<LocationEntity>> GetLocationsAsync(CancellationToken ct = default);

    public Task<LocationEntity> CreateLocationAsync(
        string name,
        string slug,
        LocationType? type,
        CancellationToken ct = default
    );
}
