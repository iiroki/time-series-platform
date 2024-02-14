namespace Iiroki.TimeSeriesPlatform.Core.Extensions;

public static class DateExtensions
{
    public static DateTime EnsureUtc(this DateTime d) =>
        d.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(d, DateTimeKind.Utc) : d.ToUniversalTime();
}
