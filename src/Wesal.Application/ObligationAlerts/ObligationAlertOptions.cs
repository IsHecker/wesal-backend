namespace Wesal.Application.ObligationAlerts;

public sealed class ObligationAlertOptions
{
    public const string SectionName = "ObligationAlerts";

    public int MaxViolationsCount { get; init; }

    public bool EnforceMaxViolationsThreshold { get; init; }
}