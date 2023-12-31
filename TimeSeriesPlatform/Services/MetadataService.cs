using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Database.Entities;
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

    public async Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct)
    {
        var integration = new IntegrationEntity { Name = name, Slug = slug };
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

    public async Task<List<TagEntity>> GetTagsAsync(CancellationToken ct) =>
        await _dbContext.Tag.AsNoTracking().ToListAsync(ct);

    public async Task<TagEntity> CreateTagAsync(string name, string slug, CancellationToken ct)
    {
        var tag = new TagEntity { Name = name, Slug = slug };
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
}
