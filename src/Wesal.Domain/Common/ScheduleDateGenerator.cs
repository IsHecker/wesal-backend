namespace Wesal.Domain.Common;

public enum ScheduleFrequency
{
    Daily,
    Weekly,
    Monthly,
    Yearly
}

public static class ScheduleDateGenerator
{
    public static DateOnly GetNextDate(DateOnly currentDate, ScheduleFrequency type)
    {
        return type switch
        {
            ScheduleFrequency.Daily => currentDate.AddDays(1),
            ScheduleFrequency.Weekly => currentDate.AddDays(7),
            ScheduleFrequency.Monthly => currentDate.AddMonths(1),
            ScheduleFrequency.Yearly => currentDate.AddYears(1),
            _ => throw new NotImplementedException()
        };
    }

    public static IEnumerable<DateOnly> GetNextDates(
        DateOnly startDate,
        ScheduleFrequency frequencyType)
    {
        // Calculate the generation limit: one month from either now or future date, whichever is later.
        var limitDate = startDate.AddMonths(1);

        for (DateOnly date = startDate; date <= limitDate; date = GetNextDate(date, frequencyType))
        {
            // if (date <= EgyptTime.Today)
            //     continue;

            yield return date;
        }
    }
}