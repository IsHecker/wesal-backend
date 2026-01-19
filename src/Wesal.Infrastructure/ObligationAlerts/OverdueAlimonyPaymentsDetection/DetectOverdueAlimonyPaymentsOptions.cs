namespace Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;

internal sealed class DetectOverdueAlimonyPaymentsOptions
{
    public const string SectionName = "BackgroundJobs:DetectOverdueAlimonyPayments";

    public bool Enabled { get; init; } = true;
    public int BatchSize { get; init; }
    public float RunIntervalInMinutes { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}