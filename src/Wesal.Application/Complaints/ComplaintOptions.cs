namespace Wesal.Application.Complaints;

public sealed class ComplaintOptions
{
    public const string SectionName = "Complaints";

    public int MaxComplaintsInMonth { get; init; }
}