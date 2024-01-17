using Iiroki.TimeSeriesPlatform.Database.Entities;

namespace Iiroki.TimeSeriesPlatform.Database.Queries;

public static class MeasurementQueries
{
    private const string IntegrationSubquery = "_integration";
    private const string TagSubquery = "_tag";
    private const string LocationSubquery = "_location";
    private static readonly string MeasurementTable = TspDbContext.ToTableName(nameof(TspDbContext.Measurement));
    private static readonly string TagTable = TspDbContext.ToTableName(nameof(TspDbContext.Tag));
    private static readonly string IntegrationTable = TspDbContext.ToTableName(nameof(TspDbContext.Integration));
    private static readonly string LocationTable = TspDbContext.ToTableName(nameof(TspDbContext.Location));

    /// <summary>
    /// <b>Params:</b><br/>
    /// 1. Integration slug<br/>
    /// 2. Tag slug<br/>
    /// 3. Location slug (nullable)<br/>
    /// 4. Version timestamp<br/>
    /// 5 + (2n - 1). Timestamp<br/>
    /// 6 + (2n - 1). Value<br/>
    /// </summary>
    public static string Upsert(int count) =>
        $"""
        WITH
            {IntegrationSubquery} AS (
                SELECT "{nameof(IntegrationEntity.Id)}"
                FROM {IntegrationTable}
                WHERE "{nameof(IntegrationEntity.Slug)}" = $1
                LIMIT 1
            ),
            {TagSubquery} AS (
                SELECT "{nameof(TagEntity.Id)}"
                FROM {TagTable}
                WHERE "{nameof(TagEntity.Slug)}" = $2
                LIMIT 1
            ),
            {LocationSubquery} AS (
                SELECT "{nameof(LocationEntity.Id)}"
                FROM {LocationTable}
                WHERE "{nameof(LocationEntity.Slug)}" = $3
                LIMIT 1
            )
        INSERT INTO {MeasurementTable} AS m (
            "{nameof(MeasurementEntity.IntegrationId)}",
            "{nameof(MeasurementEntity.TagId)}",
            "{nameof(MeasurementEntity.LocationId)}",
            "{nameof(MeasurementEntity.VersionTimestamp)}",
            "{nameof(MeasurementEntity.Timestamp)}",
            "{nameof(MeasurementEntity.Value)}"
        )
        VALUES
            {CreateValueList(count)}
        ON CONFLICT (
            "{nameof(MeasurementEntity.IntegrationId)}",
            "{nameof(MeasurementEntity.TagId)}",
            "{nameof(MeasurementEntity.LocationId)}",
            "{nameof(MeasurementEntity.Timestamp)}"
        )
        DO
            UPDATE SET
                "{nameof(MeasurementEntity.Value)}" = EXCLUDED."{nameof(MeasurementEntity.Value)}",
                "{nameof(MeasurementEntity.VersionTimestamp)}" = $4
            WHERE m."{nameof(MeasurementEntity.VersionTimestamp)}" < $4;
        """;

    private static string CreateValueList(int count) =>
        string.Join(", ", Enumerable.Range(1, count).Select(i => $"({CreateValue(i)})"));

    private static string CreateValue(int index) =>
        string.Join(
            ", ",
            $"(SELECT \"{nameof(IntegrationEntity.Id)}\" FROM {IntegrationSubquery} LIMIT 1)",
            $"(SELECT \"{nameof(TagEntity.Id)}\" FROM {TagSubquery} LIMIT 1)",
            $"(SELECT \"{nameof(LocationEntity.Id)}\" FROM {LocationSubquery} LIMIT 1)",
            "$4",
            CreateParamList(2, CalculateParamPos(4, index))
        );

    private static string CreateParamList(int count, int start) =>
        string.Join(", ", Enumerable.Range(start, count).Select(i => $"${i}"));

    private static int CalculateParamPos(int offset, int index) => offset + 2 * index - 1;
}
