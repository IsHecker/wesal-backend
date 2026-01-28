using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.VisitationSchedules;

public sealed class VisitationSchedule : Entity
{
    public Guid CourtId { get; private set; }
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid ParentId { get; private set; }

    public Guid LocationId { get; private set; }
    public VisitationFrequency Frequency { get; private set; }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public DateOnly StartDate { get; private set; }

    // When this obligation ends (could be years later)
    public DateOnly EndDate { get; private set; }

    public DateOnly? LastGeneratedDate { get; private set; } = null;

    private VisitationSchedule() { }

    public static VisitationSchedule Create(
        Guid courtId,
        Guid courtCaseId,
        Guid familyId,
        Guid parentId,
        Guid locationId,
        VisitationFrequency frequency,
        DateOnly startDate,
        DateOnly endDate,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return new VisitationSchedule
        {
            CourtId = courtId,
            CourtCaseId = courtCaseId,
            FamilyId = familyId,
            ParentId = parentId,
            LocationId = locationId,
            Frequency = frequency,
            StartDate = startDate,
            EndDate = endDate,
            StartTime = startTime,
            EndTime = endTime,
            LastGeneratedDate = null
        };
    }

    public void UpdateLastGeneratedDate(DateOnly visitationDate)
    {
        if (LastGeneratedDate.HasValue && visitationDate < LastGeneratedDate.Value)
            throw new InvalidOperationException();

        LastGeneratedDate = visitationDate;
    }

    public int GetFrequencyInDays(DateOnly targetDate)
    {
        return Frequency switch
        {
            VisitationFrequency.Daily => 1,
            VisitationFrequency.Weekly => 7,
            VisitationFrequency.Monthly => DateTime.DaysInMonth(targetDate.Year, targetDate.Month),
            _ => throw new NotImplementedException()
        };
    }
}