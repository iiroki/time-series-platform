using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    // INTEGRATIONS

    public Task<List<IntegrationEntity>> GetIntegrationsAsync(CancellationToken ct = default);

    public Task<IntegrationEntity> CreateIntegrationAsync(IntegrationData data, CancellationToken ct = default);

    // public Task<IntegrationEntity> UpdateIntegrationAsync(long id, string name, CancellationToken ct);

    public Task<bool> DeleteIntegrationAsync(long id, CancellationToken ct = default);

    // TAGS

    public Task<List<TagEntity>> GetTagsAsync(CancellationToken ct = default);

    public Task<TagEntity> CreateTagAsync(TagData data, CancellationToken ct = default);

    public Task<bool> DeleteTagAsync(long id, CancellationToken ct = default);

    // LOCATIONS

    public Task<List<LocationEntity>> GetLocationsAsync(CancellationToken ct = default);

    public Task<LocationEntity> CreateLocationAsync(LocationData data, CancellationToken ct = default);

    public Task<bool> DeleteLocationAsync(long id, CancellationToken ct = default);
}
