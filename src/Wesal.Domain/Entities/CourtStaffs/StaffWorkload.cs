using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.CourtStaffs;

public sealed class StaffWorkload : Entity
{
    public Guid CourtStaffId { get; private set; }
    public AssignmentType Type { get; private set; }
    public int LoadCount { get; private set; }

    private StaffWorkload() { }

    public static StaffWorkload Create(Guid courtStaffId, AssignmentType type)
    {
        return new StaffWorkload
        {
            CourtStaffId = courtStaffId,
            Type = type,
            LoadCount = 0
        };
    }

    public void Increment()
    {
        LoadCount++;
    }

    public void Decrement()
    {
        if (LoadCount > 0)
            LoadCount--;
    }
}