using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;

internal static class SqidsExtensions
{
    public static long DecodeSingle(this SqidsEncoder<long> sqids, string id) =>
        sqids.Decode(id) is [var @out] ? @out : throw new ArgumentException($"Sqids decoding failed for ID: {id}");
}
