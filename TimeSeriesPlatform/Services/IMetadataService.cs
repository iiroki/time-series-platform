using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMetadataService
{
    // INTEGRATIONS

    public Task<List<Integration>> GetIntegrationsAsync(CancellationToken ct = default);

    public Task<Integration> CreateIntegrationAsync(IntegrationData data, CancellationToken ct = default);

    // public Task<Integration> UpdateIntegrationAsync(long id, string name, CancellationToken ct);

    public Task<bool> DeleteIntegrationAsync(string id, CancellationToken ct = default);

    // TAGS

    public Task<List<Tag>> GetTagsAsync(CancellationToken ct = default);

    public Task<Tag> CreateTagAsync(TagData data, CancellationToken ct = default);

    public Task<bool> DeleteTagAsync(string id, CancellationToken ct = default);

    // LOCATIONS

    public Task<List<Location>> GetLocationsAsync(CancellationToken ct = default);

    public Task<Location> CreateLocationAsync(LocationData data, CancellationToken ct = default);

    public Task<bool> DeleteLocationAsync(string id, CancellationToken ct = default);
}
