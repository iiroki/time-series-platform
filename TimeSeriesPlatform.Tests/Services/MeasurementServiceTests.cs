using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.EntityFrameworkCore;
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
        var now = DateTime.UtcNow;
        var measurements = new List<MeasurementDto>
        {
            new()
            {
                Tag = TestTag,
                Data = new List<MeasurementDto.MeasurementDataDto>
                {
                    new() { Timestamp = now.AddMinutes(-5), Value = 1.23 },
                    new() { Timestamp = now.AddMinutes(-4), Value = 2.34 },
                    new() { Timestamp = now.AddMinutes(-3), Value = 3.45 },
                    new() { Timestamp = now.AddMinutes(-2), Value = 4.56 },
                    new() { Timestamp = now.AddMinutes(-1), Value = 5.67 },
                }
            }
        };

        await _measurementService.SaveMeasurementsAsync(measurements, TestIntegration, CancellationToken.None);

        var dbContext = CreateDbContext();
        var result = await GetResultAsync();
        Assert.That(result, Has.Count.EqualTo(measurements.Sum(m => m.Data.Count)));
        AssertMeasurements(measurements, result, TestIntegration);
    }

    private static async Task<List<MeasurementEntity>> GetResultAsync() =>
        await CreateDbContext()
            .Measurement
            .AsNoTracking()
            .Include(m => m.Tag)
            .Include(m => m.Integration)
            .ToListAsync();

    private static void AssertMeasurements(
        IList<MeasurementDto> expected,
        IList<MeasurementEntity> actual,
        string integration,
        DateTime? UpdateTimestamp = null
    )
    {
        var expectedFlattened = expected
            .SelectMany(e => e.Data.Select(d => new MeasurementDto { Tag = e.Tag, Data = new[] { d } }))
            .ToList();

        var expectedSorted = expectedFlattened.OrderBy(m => m.Data.First().Timestamp).ToList();
        var actualSorted = actual.OrderBy(m => m.Timestamp).ToList();

        Assert.That(actualSorted, Has.Count.EqualTo(expectedSorted.Count));
        Assert.Multiple(() =>
        {
            for (var i = 0; i < actualSorted.Count; ++i)
            {
                var a = actualSorted[i];
                var e = expectedSorted[i];
                var data = e.Data.First();
                Assert.That(a.Integration.Slug, Is.EqualTo(integration));
                Assert.That(a.Tag.Slug, Is.EqualTo(e.Tag));
                Assert.That(a.Timestamp, Is.EqualTo(data.Timestamp).Within(TimeSpan.FromMilliseconds(0)));
                Assert.That(a.Value, Is.EqualTo(data.Value));
            }
        });
    }
}
