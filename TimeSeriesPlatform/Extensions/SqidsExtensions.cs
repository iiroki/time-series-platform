using Sqids;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class SqidsExtensions
{
    public static long DecodeFirst(this SqidsEncoder<long> sqids, string id)
    {
        var decoded = sqids.Decode(id);
        return decoded.Count > 0 ? decoded[0] : throw new ArgumentException($"Sqids decoding failed for ID: {id}");
    }
}
