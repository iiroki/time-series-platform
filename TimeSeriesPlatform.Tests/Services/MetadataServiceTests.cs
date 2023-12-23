using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Iiroki.TimeSeriesPlatform.Tests.Servces;

public class MetadataServiceTests : DatabaseTestBase
{
    private IMetadataService _metadataService = null!;

    [SetUp]
    public void SetupServiceAsync()
    {
        _metadataService = new MetadataService(CreateDbContext(), Substitute.For<ILogger<MetadataService>>());
    }

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
}
