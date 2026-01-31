namespace Wesal.Infrastructure.PaymentsDue.PaymentDueReminder;

internal sealed class PaymentDueReminderOptions
{
    public const string SectionName = "BackgroundJobs:PaymentDueReminder";

    public bool Enabled { get; init; }
    public float RunIntervalInMinutes { get; init; }
    public int BatchSize { get; init; }
    public int ReminderDaysBeforeDueDate { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}