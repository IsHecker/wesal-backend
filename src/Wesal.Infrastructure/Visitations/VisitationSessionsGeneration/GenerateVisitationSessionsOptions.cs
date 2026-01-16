namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

internal sealed class GenerateVisitationSessionsOptions
{
    public const string SectionName = "BackgroundJobs:GenerateVisitationSessions";

    public bool Enabled { get; init; } = true;
    public float RunIntervalInMinutes { get; init; }
    public int BatchSize { get; init; }
    public int GenerateForNextDays { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}