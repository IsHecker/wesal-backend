using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.VisitationSchedules;

public sealed class VisitationSchedule : Entity
{
    public Guid CCCourtCaseId { get; private set; }
    public Guid CCFamilyId { get; private set; }
    public Guid CCParentId { get; private set; }

    public Guid CCLocationId { get; private set; }
    public int StartDayInMonth { get; private set; }
    public VisitationFrequency Frequency { get; private set; }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public DateOnly? LastGeneratedDate { get; private set; } = null;

    private VisitationSchedule() { }

    public static VisitationSchedule Create(
        Guid courtCaseId,
        Guid locationId,
        int startDayInMonth,
        VisitationFrequency frequency,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return new VisitationSchedule
        {
            CCCourtCaseId = courtCaseId,
            CCLocationId = locationId,
            StartDayInMonth = startDayInMonth,
            Frequency = frequency,
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

    public int GetFrequencyInDays()
    {
        return Frequency switch
        {
            VisitationFrequency.Daily => 1,
            VisitationFrequency.Weekly => 7,
            VisitationFrequency.Monthly => DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month),
            _ => throw new NotImplementedException()
        };
    }
}