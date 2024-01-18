using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Models.Internal;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class PostgresExtensions
{
    public static async Task<MeasurementChange?> ToMeasurementChange(this UpdateMessage msg) =>
        await ToMeasurementChangeInternal(msg.NewRow, msg.Relation.Columns);

    public static async Task<MeasurementChange?> ToMeasurementChange(this InsertMessage msg) =>
        await ToMeasurementChangeInternal(msg.NewRow, msg.Relation.Columns);

    public static async Task<string> ToStr(this ReplicationValue value) => await value.Get<string>();

    public static async Task<long> ToLong(this ReplicationValue value) => long.Parse(await value.ToStr());

    public static async Task<double> ToDouble(this ReplicationValue value) => double.Parse(await value.ToStr());

    public static async Task<DateTime> ToDateTime(this ReplicationValue value) =>
        DateTime.Parse(await value.ToStr()).ToUniversalTime();

    private static async Task<MeasurementChange?> ToMeasurementChangeInternal(
        this ReplicationTuple row,
        IReadOnlyList<RelationMessage.Column> columns
    )
    {
        var columnsWithIndex = columns
            .Select((c, i) => KeyValuePair.Create(i, c))
            .ToDictionary(p => p.Key, p => p.Value);

        var integrationIdIndex = columnsWithIndex
            .FirstOrDefault(c => c.Value.ColumnName == nameof(MeasurementEntity.IntegrationId))
            .Key;

        var tagIdIndex = columnsWithIndex
            .FirstOrDefault(c => c.Value.ColumnName == nameof(MeasurementEntity.TagId))
            .Key;

        var timestampIndex = columnsWithIndex
            .FirstOrDefault(c => c.Value.ColumnName == nameof(MeasurementEntity.Timestamp))
            .Key;

        var valueIndex = columnsWithIndex
            .FirstOrDefault(c => c.Value.ColumnName == nameof(MeasurementEntity.Value))
            .Key;

        var builder = new MeasurementChange.Builder();
        var currentColIndex = 0;
        await foreach (var current in row)
        {
            if (currentColIndex == integrationIdIndex)
            {
                builder.IntegrationId = await current.ToLong();
            }
            else if (currentColIndex == tagIdIndex)
            {
                builder.TagId = await current.ToLong();
            }
            else if (currentColIndex == timestampIndex)
            {
                builder.Timestamp = await current.ToDateTime();
            }
            else if (currentColIndex == valueIndex)
            {
                builder.Value = await current.ToDouble();
            }

            ++currentColIndex;
        }

        return builder.Build();
    }
}
