using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Iiroki.TimeSeriesPlatform.Tests.Services;

public class MeasurementServiceTests : DatabaseTestBase
{
    private const string TestIntegration = "test-integration";
    private const string TestTagPrefix = "test-tag";
    private static readonly string[] TestTags = Enumerable.Range(1, 5).Select(i => $"{TestTagPrefix}_{i}").ToArray();

    private IMeasurementService _measurementService = null!;

    [SetUp]
    public async Task SetupAsync()
    {
        _measurementService = new MeasurementService(CreateDbSource(), Substitute.For<ILogger<MeasurementService>>());

        var dbContext = CreateDbContext();
        dbContext.AddRange(
            TestTags.Select((t, i) => new TagEntity { Name = $"{nameof(MeasurementService)} Tag {1 + i}", Slug = t })
        );

        dbContext
            .Integration
            .Add(new IntegrationEntity { Name = $"{nameof(MeasurementService)} Integration", Slug = TestIntegration });

        await dbContext.SaveChangesAsync();
    }

    [Test]
    public async Task MeasurementService_SaveMeasurements_Insert_Ok()
    {
        var now = DateTime.UtcNow;
        var measurements = TestTags
            .Select(
                (t, i) =>
                    new MeasurementDto()
                    {
                        Tag = t,
                        Data = new List<MeasurementDto.MeasurementDataDto>
                        {
                            new() { Timestamp = now.AddMinutes(-5), Value = 1.23 + i * 10 },
                            new() { Timestamp = now.AddMinutes(-4), Value = 2.34 + i * 10 },
                            new() { Timestamp = now.AddMinutes(-3), Value = 3.45 + i * 10 },
                            new() { Timestamp = now.AddMinutes(-2), Value = 4.56 + i * 10 },
                            new() { Timestamp = now.AddMinutes(-1), Value = 5.67 + i * 10 },
                        }
                    }
            )
            .ToList();

        await _measurementService.SaveMeasurementsAsync(measurements, TestIntegration, CancellationToken.None);

        var result = await GetResultAsync();
        AssertMeasurements(measurements, result, TestIntegration);
    }

    [Test]
    public async Task MeasurementService_SaveMeasurements_Update_Ok()
    {
        var now = DateTime.UtcNow;
        var tag = TestTags.First();
        var measurements = new List<MeasurementDto>
        {
            new()
            {
                Tag = tag,
                Data = new List<MeasurementDto.MeasurementDataDto>
                {
                    new() { Timestamp = now.AddMinutes(-2), Value = 123.45 }, // <-- This value should be updated
                    new() { Timestamp = now.AddMinutes(-1), Value = 543.21 }
                }
            }
        };

        var updatedMeasurements = new List<MeasurementDto>
        {
            new()
            {
                Tag = tag,
                Data = new List<MeasurementDto.MeasurementDataDto>
                {
                    new() { Timestamp = measurements.First().Data.First().Timestamp, Value = 987.65 }
                }
            }
        };

        await _measurementService.SaveMeasurementsAsync(measurements, TestIntegration, CancellationToken.None);
        await _measurementService.SaveMeasurementsAsync(updatedMeasurements, TestIntegration, CancellationToken.None);

        var result = await GetResultAsync();
        var expected = new List<MeasurementDto>
        {
            new() { Tag = tag, Data = new[] { measurements.First().Data.Last() } },
            new() { Tag = tag, Data = updatedMeasurements.First().Data }
        };

        AssertMeasurements(expected, result, TestIntegration);
    }

    [Test]
    [Ignore("Feature not yet implemented")]
    public Task MeasurementService_SaveMeasurements_Update_UpdateTimestamp_Ok()
    {
        return Task.CompletedTask;
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
        DateTime? updateTimestamp = null
    )
    {
        var expectedFlattened = expected
            .SelectMany(e => e.Data.Select(d => new MeasurementDto { Tag = e.Tag, Data = new[] { d } }))
            .ToList();

        var expectedSorted = expectedFlattened.OrderBy(m => m.Tag).ThenBy(m => m.Data.First().Timestamp).ToList();
        var actualSorted = actual.OrderBy(m => m.Tag.Slug).ThenBy(m => m.Timestamp).ToList();

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
                if (updateTimestamp.HasValue)
                {
                    Assert.That(a.UpdateTimestamp, Is.EqualTo(updateTimestamp.Value));
                }
            }
        });
    }
}
