using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Domain.Entities.VisitationSchedules;

public sealed class VisitationSchedule : Entity
{
    public Guid CourtId { get; private set; }
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid CustodialParentId { get; private set; }
    public string CustodialNationalId { get; private set; } = null!;
    public Guid NonCustodialParentId { get; private set; }
    public string NonCustodialNationalId { get; private set; } = null!;
    public Guid LocationId { get; private set; }

    public VisitationFrequency Frequency { get; private set; }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public DateOnly StartDate { get; private set; }

    // When this obligation ends (could be years later)
    public DateOnly? EndDate { get; private set; }

    public DateOnly? LastGeneratedDate { get; private set; }

    public bool IsStopped { get; private set; }

    private VisitationSchedule() { }

    public static VisitationSchedule Create(
        Guid courtId,
        Guid courtCaseId,
        Guid familyId,
        Parent custodialParent,
        Parent nonCustodialParent,
        Guid locationId,
        VisitationFrequency frequency,
        DateOnly startDate,
        DateOnly? endDate,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return new VisitationSchedule
        {
            CourtId = courtId,
            CourtCaseId = courtCaseId,
            FamilyId = familyId,
            CustodialParentId = custodialParent.Id,
            CustodialNationalId = custodialParent.NationalId,
            NonCustodialParentId = nonCustodialParent.Id,
            NonCustodialNationalId = nonCustodialParent.NationalId,
            LocationId = locationId,
            Frequency = frequency,
            StartDate = startDate,
            EndDate = endDate,
            StartTime = startTime,
            EndTime = endTime,
            LastGeneratedDate = null
        };
    }

    public void UpdateLastGeneratedDate(DateOnly lastDate)
    {
        if (LastGeneratedDate.HasValue && lastDate < LastGeneratedDate.Value)
            throw new InvalidOperationException();

        LastGeneratedDate = lastDate;
    }

    public void Stop() => IsStopped = true;

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

    public void Update(
        Guid locationId,
        VisitationFrequency frequency,
        TimeOnly startTime,
        TimeOnly endTime,
        DateOnly startDate,
        DateOnly? endDate)
    {
        LocationId = locationId;
        Frequency = frequency;
        StartTime = startTime;
        EndTime = endTime;
        StartDate = startDate;
        EndDate = endDate;
        LastGeneratedDate = null;
    }
}