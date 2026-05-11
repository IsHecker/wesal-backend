using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Domain.Entities.Families;

public sealed class Family : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FatherId { get; private set; }
    public Guid MotherId { get; private set; }
    public Guid? AssignedStaffId { get; private set; }
    public FamilyStatus Status { get; private set; }
    public DateTime? ResolvedAt { get; private set; }

    public Parent Father { get; private set; } = null!;
    public Parent Mother { get; private set; } = null!;
    public ICollection<Child> Children { get; private set; } = [];

    private Family() { }

    public static Family Create(Guid courtId, Guid fatherId, Guid motherId, Guid? assignedStaffId = null)
    {
        return new Family
        {
            CourtId = courtId,
            FatherId = fatherId,
            MotherId = motherId,
            AssignedStaffId = assignedStaffId,
            Status = FamilyStatus.Active
        };
    }

    public void Resolve(FamilyStatus status)
    {
        Status = status;
        ResolvedAt = EgyptTime.Now;
    }

    public void Reactivate()
    {
        Status = FamilyStatus.Active;
        ResolvedAt = null;
    }
}