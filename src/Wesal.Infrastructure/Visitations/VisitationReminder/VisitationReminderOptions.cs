namespace Wesal.Infrastructure.Visitations.VisitationReminder;

internal sealed class VisitationReminderOptions
{
    public const string SectionName = "BackgroundJobs:VisitationReminder";

    public bool Enabled { get; init; } = true;
    public float RunIntervalInMinutes { get; init; }
    public int BatchSize { get; init; }
    public int ReminderDaysBeforeVisitation { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}