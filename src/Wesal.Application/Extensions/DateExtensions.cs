namespace Wesal.Application.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);
}