using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Services;
using Iiroki.TimeSeriesPlatform.Services.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Iiroki.TimeSeriesPlatform.Tests.Services;

public class MetadataServiceTests : DatabaseTestBase
{
    private IMetadataService _metadataService = null!;

    [SetUp]
    public void SetupAsync()
    {
        _metadataService = new MetadataService(CreateDbContext(), Substitute.For<ILogger<MetadataService>>());
    }

    #region Integrations
    [Test]
    public async Task MetadataService_GetIntegrationsAsync_Ok()
    {
        var integrations = new[]
        {
            new IntegrationEntity { Name = "First Integration", Slug = "1st" },
            new IntegrationEntity { Name = "Second Integration", Slug = "2nd" },
            new IntegrationEntity { Name = "Third Integration", Slug = "3rd" }
        };

        var dbContext = CreateDbContext();
        dbContext.Integration.AddRange(integrations);
        await dbContext.SaveChangesAsync();

        var result = await _metadataService.GetIntegrationsAsync(CancellationToken.None);
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
    public async Task MetadataService_CreateIntegrationAsync_Ok()
    {
        var name = "Test Integration";
        var slug = "integration";
        var result = await _metadataService.CreateIntegrationAsync(name, slug, CancellationToken.None);
        var integrations = await _metadataService.GetIntegrationsAsync(CancellationToken.None);
        var integration = integrations.FirstOrDefault(i => i.Id == result.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Slug, Is.EqualTo(slug));
            Assert.That(integration, Is.Not.Null);
        });
    }

    [Test]
    public async Task MetadataService_CreateIntegrationAsync_Unique_Error()
    {
        var slug = "integration";
        await _metadataService.CreateIntegrationAsync("Test Integration", slug, CancellationToken.None);

        Assert.ThrowsAsync<MetadataServiceException>(
            async () =>
                await _metadataService.CreateIntegrationAsync("Duplicate Integration", slug, CancellationToken.None)
        );
    }
    #endregion

    #region Tags
    [Test]
    public async Task MetadataService_GetTagsAsync_Ok()
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

        var dbContext = CreateDbContext();
        dbContext.Tag.AddRange(tags);
        await dbContext.SaveChangesAsync();

        var result = await _metadataService.GetTagsAsync(CancellationToken.None);
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
    public async Task MetadataService_CreateTagAsync_Ok()
    {
        var name = "Test Tag";
        var slug = "tag";
        var result = await _metadataService.CreateTagAsync(name, slug, CancellationToken.None);
        var tags = await _metadataService.GetTagsAsync(CancellationToken.None);
        var tag = tags.FirstOrDefault(i => i.Id == result.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Slug, Is.EqualTo(slug));
            Assert.That(tag, Is.Not.Null);
        });
    }

    [Test]
    public async Task MetadataService_CreateTagAsync_Unique_Error()
    {
        var slug = "tag";
        await _metadataService.CreateTagAsync("Test Tag", slug, CancellationToken.None);

        Assert.ThrowsAsync<MetadataServiceException>(
            async () => await _metadataService.CreateTagAsync("Duplicate Tag", slug, CancellationToken.None)
        );
    }
    #endregion
}
