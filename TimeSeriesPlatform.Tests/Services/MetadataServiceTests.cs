using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Services;
using Iiroki.TimeSeriesPlatform.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Tests.Services;

public class MetadataServiceTests : DatabaseTestBase
{
    private readonly SqidsEncoder<long> _sqids = new();
    private IMetadataService _metadataService = null!;

    [SetUp]
    public void SetupAsync()
    {
        _metadataService = new MetadataService(CreateDbContext(), _sqids, Substitute.For<ILogger<MetadataService>>());
    }

    #region Integrations
    [Test]
    public async Task MetadataService_GetIntegrations_Ok()
    {
        var integrations = new[]
        {
            new IntegrationEntity { Name = "First Integration", Slug = "1st" },
            new IntegrationEntity { Name = "Second Integration", Slug = "2nd" },
            new IntegrationEntity { Name = "Third Integration", Slug = "3rd" }
        };

        await using var dbContext = CreateDbContext();
        dbContext.Integration.AddRange(integrations);
        await dbContext.SaveChangesAsync();

        var result = await _metadataService.GetIntegrationsAsync();
        var expected = integrations.OrderBy(i => i.Slug).ToList();
        var actual = result.OrderBy(i => i.Slug).ToList();

        Assert.That(actual, Has.Count.EqualTo(expected.Count));
        Assert.Multiple(() =>
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                var e = expected[i];
                var a = actual[i];
                Assert.That(a.Name, Is.EqualTo(e.Name));
                Assert.That(a.Slug, Is.EqualTo(e.Slug));
            }
        });
    }

    [Test]
    public async Task MetadataService_CreateIntegration_Ok()
    {
        var name = "Test Integration";
        var slug = "integration";
        var result = await _metadataService.CreateIntegrationAsync(new() { Name = name, Slug = slug });
        var integrations = await _metadataService.GetIntegrationsAsync();
        var integration = integrations.FirstOrDefault(i => i.Id == result.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Slug, Is.EqualTo(slug));
            Assert.That(integration, Is.Not.Null);
        });
    }

    [Test]
    public async Task MetadataService_CreateIntegration_Unique_Error()
    {
        var slug = "integration";
        await _metadataService.CreateIntegrationAsync(new() { Name = "Test Integration", Slug = slug });
        Assert.ThrowsAsync<MetadataServiceException>(
            async () =>
                await _metadataService.CreateIntegrationAsync(new() { Name = "Duplicate Integration", Slug = slug })
        );
    }

    // TODO: Delete test
    #endregion

    #region Tags
    [Test]
    public async Task MetadataService_GetTags_Ok()
    {
        var tags = new[]
        {
            new TagEntity
            {
                Id = 1,
                Name = "Test Tag 1",
                Slug = "test-tag-1"
            },
            new TagEntity
            {
                Id = 2,
                Name = "Test Tag 2",
                Slug = "test-tag-2"
            },
            new TagEntity
            {
                Id = 3,
                Name = "Test Tag 3",
                Slug = "test-tag-3"
            },
            new TagEntity
            {
                Id = 4,
                Name = "Test Tag 4",
                Slug = "test-tag-4"
            },
            new TagEntity
            {
                Id = 5,
                Name = "Test Tag 5",
                Slug = "test-tag-5"
            }
        };

        await using var dbContext = CreateDbContext();
        dbContext.Tag.AddRange(tags);
        await dbContext.SaveChangesAsync();

        var result = await _metadataService.GetTagsAsync();
        var expected = tags.OrderBy(i => i.Slug).ToList();
        var actual = result.OrderBy(i => i.Slug).ToList();

        Assert.That(actual, Has.Count.EqualTo(expected.Count));
        Assert.Multiple(() =>
        {
            for (var i = 0; i < expected.Count; ++i)
            {
                var e = expected[i];
                var a = actual[i];
                Assert.That(a.Name, Is.EqualTo(e.Name));
                Assert.That(a.Slug, Is.EqualTo(e.Slug));
            }
        });
    }

    [Test]
    public async Task MetadataService_CreateTag_Ok()
    {
        var name = "Test Tag";
        var slug = "tag";
        var result = await _metadataService.CreateTagAsync(new() { Name = name, Slug = slug });
        var tags = await _metadataService.GetTagsAsync();
        var tag = tags.FirstOrDefault(i => i.Id == result.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Slug, Is.EqualTo(slug));
            Assert.That(tag, Is.Not.Null);
        });
    }

    [Test]
    public async Task MetadataService_CreateTag_Unique_Error()
    {
        var slug = "tag";
        await _metadataService.CreateTagAsync(new() { Name = "Test Tag", Slug = slug });

        Assert.ThrowsAsync<MetadataServiceException>(
            async () => await _metadataService.CreateTagAsync(new() { Name = "Duplicate Tag", Slug = slug })
        );
    }

    [Test]
    public async Task MetadataService_DeleteTag_Ok()
    {
        var tag = new TagEntity
        {
            Id = 123,
            Name = "Test Tag",
            Slug = "test"
        };
        await using var dbContext = CreateDbContext();
        dbContext.Tag.Add(tag);
        await dbContext.SaveChangesAsync();

        var result = await _metadataService.DeleteTagAsync(_sqids.Encode(tag.Id));
        Assert.Multiple(async () =>
        {
            Assert.That(result, Is.True);
            Assert.That(await dbContext.Tag.ToListAsync(), Is.Empty);
        });
    }
    #endregion

    // TODO: Location tests
}
