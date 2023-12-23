using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Database.Entities;
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

    public Task<IntegrationEntity> CreateIntegrationAsync(string name, string slug, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<TagEntity>> GetTagsAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<TagEntity> CreateTagAsync(string todo, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
