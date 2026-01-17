namespace Wesal.Application.Visitations;

public sealed class VisitationOptions
{
    public const string SectionName = "VisitationOptions";

    public int GracePeriodMinutes { get; init; }
}