namespace Wesal.Application.Visitations;

public sealed class VisitationOptions
{
    public const string SectionName = "Visitations";

    public int GracePeriodMinutes { get; init; }
}