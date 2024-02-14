using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Core.Extensions;

public static class SqidsExtensions
{
    private const string SqidsAlphabets = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

    /// <summary>
    /// Adds Sqids encoder to services.
    /// </summary>
    public static void AddSqids(this IServiceCollection services) =>
        services.AddSingleton(new SqidsEncoder<long>(new() { Alphabet = SqidsAlphabets, MinLength = 6 }));

    public static long DecodeSingle(this SqidsEncoder<long> sqids, string id) =>
        sqids.Decode(id) is [var @out] ? @out : throw new ArgumentException($"Sqids decoding failed for ID: {id}");
}
