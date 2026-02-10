namespace Wesal.Domain.Common;

public static class EgyptTime
{
    private static readonly TimeZoneInfo EgyptZone =
        TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

    public static DateTime Now
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EgyptZone);

    public static DateOnly Today
        => DateOnly.FromDateTime(Now);

    public static DateTime ToUtc(this DateTime egyptTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(egyptTime, EgyptZone);
    }

    public static DateTime ToEgypt(this DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, EgyptZone);
    }
}