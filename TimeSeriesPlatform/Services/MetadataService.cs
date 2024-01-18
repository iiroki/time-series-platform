using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models;
using Iiroki.TimeSeriesPlatform.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Services;

public class MetadataService : IMetadataService
{
    private readonly TspDbContext _dbContext;
    private readonly ILogger<MetadataService> _logger;

    public MetadataService(TspDbContext dbContext, ILogger<MetadataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<IntegrationEntity>> GetIntegrationsAsync(CancellationToken ct) =>
        await _dbContext.Integration.AsNoTracking().ToListAsync(ct);

    public async Task<IntegrationEntity> CreateIntegrationAsync(IntegrationData data, CancellationToken ct)
    {
        var integration = new IntegrationEntity
        {
            Name = data.Name,
            Slug = data.Slug,
            VersionTimestamp = DateTime.UtcNow
        };

        _dbContext.Integration.Add(integration);
        try
        {
            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Created integration: {I}", integration);
            return integration;
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create integration: {integration}", ex);
        }
    }

    public async Task<bool> DeleteIntegrationAsync(long id, CancellationToken ct)
    {
        _dbContext.Integration.Remove(new IntegrationEntity { Id = id });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted integration with ID: {Id}", id);
            return true;
        }

        return false;
    }

    public async Task<List<TagEntity>> GetTagsAsync(CancellationToken ct) =>
        await _dbContext.Tag.AsNoTracking().ToListAsync(ct);

    public async Task<TagEntity> CreateTagAsync(TagData data, CancellationToken ct)
    {
        var tag = new TagEntity
        {
            Name = data.Name,
            Slug = data.Slug,
            VersionTimestamp = DateTime.UtcNow
        };

        _dbContext.Tag.Add(tag);
        try
        {
            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Created tag: {T}", tag);
            return tag;
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create tag: {tag}", ex);
        }
    }

    public async Task<bool> DeleteTagAsync(long id, CancellationToken ct)
    {
        _dbContext.Tag.Remove(new TagEntity { Id = id });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted tag with ID: {Id}", id);
            return true;
        }

        return false;
    }

    public async Task<List<LocationEntity>> GetLocationsAsync(CancellationToken ct) =>
        await _dbContext.Location.AsNoTracking().ToListAsync(ct);

    public async Task<LocationEntity> CreateLocationAsync(LocationData data, CancellationToken ct)
    {
        var location = new LocationEntity
        {
            Name = data.Name,
            Slug = data.Slug,
            Type = data.Type,
            VersionTimestamp = DateTime.UtcNow
        };

        _dbContext.Location.Add(location);
        try
        {
            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Created location: {L}", location);
            return location;
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create location: {location}", ex);
        }
    }

    public async Task<bool> DeleteLocationAsync(long id, CancellationToken ct)
    {
        _dbContext.Location.Remove(new() { Id = id });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted location with ID: {Id}", id);
            return true;
        }

        return false;
    }
}
