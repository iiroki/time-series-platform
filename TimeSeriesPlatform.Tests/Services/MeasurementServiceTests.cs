using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Iiroki.TimeSeriesPlatform.Tests.Services;

public class MeasurementServiceTests : DatabaseTestBase
{
    private const string TestTag = "test-tag";
    private const string TestIntegration = "test-integration";

    private IMeasurementService _measurementService = null!;

    [SetUp]
    public async Task SetupAsync()
    {
        _measurementService = new MeasurementService(CreateDbSource(), Substitute.For<ILogger<MeasurementService>>());

        var dbContext = CreateDbContext();
        dbContext.Tag.Add(new TagEntity { Name = $"{nameof(MeasurementService)} Tag", Slug = TestTag });
        dbContext
            .Integration
            .Add(new IntegrationEntity { Name = $"{nameof(MeasurementService)} Integration", Slug = TestIntegration });

        await dbContext.SaveChangesAsync();
    }

    [Test]
    public async Task MeasurementService_SaveMeasurements_Todo()
    {
        // TODO
    }
}
