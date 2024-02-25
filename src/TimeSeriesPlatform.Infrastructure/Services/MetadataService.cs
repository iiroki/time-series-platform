using Iiroki.TimeSeriesPlatform.Application.Extensions;
using Iiroki.TimeSeriesPlatform.Domain.Models;
using Iiroki.TimeSeriesPlatform.Domain.Services;
using Iiroki.TimeSeriesPlatform.Domain.Services.Exceptions;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;
using Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Services;

internal class MetadataService(TspDbContext dbContext, SqidsEncoder<long> sqids, ILogger<MetadataService> logger)
    : IMetadataService
{
    private readonly TspDbContext _dbContext = dbContext;
    private readonly SqidsEncoder<long> _sqids = sqids;
    private readonly ILogger<MetadataService> _logger = logger;

    public async Task<List<Integration>> GetIntegrationsAsync(CancellationToken ct) =>
        (await _dbContext.Integration.AsNoTracking().ToListAsync(ct)).ToDomain(_sqids).ToList();

    public async Task<Integration> CreateIntegrationAsync(IntegrationData data, CancellationToken ct)
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
            return integration.ToDomain(_sqids);
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create integration: {integration}", ex);
        }
    }

    public async Task<bool> DeleteIntegrationAsync(string id, CancellationToken ct)
    {
        var decodedId = _sqids.DecodeSingle(id);
        _dbContext.Integration.Remove(new IntegrationEntity { Id = decodedId });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted integration with ID: {Id}", decodedId);
            return true;
        }

        return false;
    }

    public async Task<List<Tag>> GetTagsAsync(CancellationToken ct) =>
        (await _dbContext.Tag.AsNoTracking().ToListAsync(ct)).ToDomain(_sqids).ToList();

    public async Task<Tag> CreateTagAsync(TagData data, CancellationToken ct)
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
            return tag.ToDomain(_sqids);
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create tag: {tag}", ex);
        }
    }

    public async Task<bool> DeleteTagAsync(string id, CancellationToken ct)
    {
        var decodedId = _sqids.DecodeSingle(id);
        _dbContext.Tag.Remove(new TagEntity { Id = decodedId });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted tag with ID: {Id}", decodedId);
            return true;
        }

        return false;
    }

    public async Task<List<Location>> GetLocationsAsync(CancellationToken ct) =>
        (await _dbContext.Location.AsNoTracking().ToListAsync(ct)).ToDomain(_sqids).ToList();

    public async Task<Location> CreateLocationAsync(LocationData data, CancellationToken ct)
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
            return location.ToDomain(_sqids);
        }
        catch (Exception ex)
        {
            throw new MetadataServiceException($"Could not create location: {location}", ex);
        }
    }

    public async Task<bool> DeleteLocationAsync(string id, CancellationToken ct)
    {
        var decodedId = _sqids.DecodeSingle(id);
        _dbContext.Location.Remove(new() { Id = decodedId });
        if (await _dbContext.SaveDeleteAsync(ct))
        {
            _logger.LogInformation("Deleted location with ID: {Id}", decodedId);
            return true;
        }

        return false;
    }
}
