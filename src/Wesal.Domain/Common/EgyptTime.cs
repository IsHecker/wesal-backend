using System.Runtime.InteropServices;

namespace Wesal.Domain.Common;

public static class EgyptTime
{
    private static readonly TimeZoneInfo EgyptZone =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")
            : TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");


    public static TimeSpan Offset { get; set; } = TimeSpan.Zero;
    public static DateTime UtcNow => DateTime.UtcNow.Add(Offset);

    public static DateTime Now
        => TimeZoneInfo.ConvertTimeFromUtc(UtcNow, EgyptZone);

    public static DateOnly Today
        => DateOnly.FromDateTime(Now);

    public static DateTime ToUtc(this DateTime egyptTime)
    {
        // We always treat the input as Egypt wall-clock time, regardless of its Kind.
        // This handles cases where client tools might append 'Z' or treat the time as Local,
        // but the user's intent is to specify the actual time in Egypt.
        var unspecified = DateTime.SpecifyKind(egyptTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, EgyptZone);
    }

    public static DateTime ToEgypt(this DateTime utcTime)
    {
        if (utcTime.Kind == DateTimeKind.Utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, EgyptZone);
        }

        var utc = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utc, EgyptZone);
    }
}