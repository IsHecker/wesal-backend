using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Domain.Entities.Visitations;

public sealed class Visitation : Entity
{
    public Guid CCFamilyId { get; private set; }
    public Guid CCParentId { get; private set; }
    public Guid CCLocationId { get; private set; }
    public Guid CCVisitationScheduleId { get; private set; }
    public Guid CCVerifiedById { get; private set; }

    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public DateTime? VisitedAt { get; private set; } = null!;
    public VisitationStatus Status { get; private set; }

    public bool IsCheckedIn { get; private set; }
    public DateTime? CheckedInAt { get; private set; } = null;

    private Visitation() { }

    public static Visitation Create(VisitationSchedule schedule, DateOnly visitationDate)
    {
        return new Visitation
        {
            CCFamilyId = schedule.CCFamilyId,
            CCParentId = schedule.CCParentId,
            CCLocationId = schedule.CCLocationId,
            CCVisitationScheduleId = schedule.Id,
            Date = visitationDate,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime
        };
    }
}