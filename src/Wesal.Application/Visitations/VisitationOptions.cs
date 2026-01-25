namespace Wesal.Application.Visitations;

public sealed class VisitationOptions
{
    public const string SectionName = "Visitations";

    public int CheckInGracePeriodMinutes { get; init; }
    public int CheckOutGracePeriodMinutes { get; init; }
}