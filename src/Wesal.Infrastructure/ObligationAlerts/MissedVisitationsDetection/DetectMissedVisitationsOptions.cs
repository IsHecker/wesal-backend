namespace Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;

internal sealed class DetectMissedVisitationsOptions
{
    public const string SectionName = "BackgroundJobs:DetectMissedVisitations";

    public bool Enabled { get; init; }
    public int BatchSize { get; init; }
    public float RunIntervalInMinutes { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}