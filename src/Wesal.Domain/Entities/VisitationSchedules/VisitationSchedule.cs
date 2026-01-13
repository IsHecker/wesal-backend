using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Domain.Entities.VisitationSchedules;

public sealed class VisitationSchedule : Entity
{
    public Guid CourtCaseId { get; private set; }
    public Guid LocationId { get; private set; }

    public VisitationFrequency Frequency { get; private set; }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public CourtCase CourtCase { get; private set; } = null!;
    public VisitationLocation Location { get; private set; } = null!;
    public ICollection<Visitation> Visitations { get; private set; } = [];

    private VisitationSchedule() { }

    public static VisitationSchedule Create(
        Guid courtCaseId,
        Guid locationId,
        VisitationFrequency frequency,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return new VisitationSchedule
        {
            CourtCaseId = courtCaseId,
            LocationId = locationId,
            Frequency = frequency,
            StartTime = startTime,
            EndTime = endTime,
        };
    }
}