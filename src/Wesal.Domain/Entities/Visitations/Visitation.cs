using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Domain.Entities.Visitations;

public sealed class Visitation : Entity
{
    public Guid FamilyId { get; private set; }
    public Guid ParentId { get; private set; }
    public Guid LocationId { get; private set; }

    public Guid VisitationScheduleId { get; private set; }

    public DateTime ScheduledVisitAt { get; private set; }
    public DateTime? VisitedAt { get; private set; } = null!;

    public VisitationStatus Status { get; private set; }
    public bool IsCheckedIn { get; private set; }
    public DateTime? CheckedInAt { get; private set; } = null;

    public Guid VerifiedById { get; private set; }

    public Family Family { get; private set; } = null!;
    public Parent Parent { get; private set; } = null!;
    public VisitationLocation Location { get; private set; } = null!;
    public VisitationSchedule VisitationSchedule { get; private set; } = null!;
    public User VerifiedBy { get; private set; } = null!;

    private Visitation() { }

    public static Visitation Create(
        Guid familyId,
        Guid parentId,
        Guid locationId,
        Guid visitationScheduleId,
        Guid verifiedById,
        DateTime scheduledVisitAt,
        VisitationStatus status,
        bool isCheckedIn)
    {
        return new Visitation
        {
            FamilyId = familyId,
            ParentId = parentId,
            LocationId = locationId,
            VisitationScheduleId = visitationScheduleId,
            VerifiedById = verifiedById,
            ScheduledVisitAt = scheduledVisitAt,
            Status = status,
            IsCheckedIn = isCheckedIn,
        };
    }
}