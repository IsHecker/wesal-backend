using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.FamilyCourts;

namespace Wesal.Domain.Entities.CourtStaffs;

public sealed class CourtStaff : Entity, IHasUserId
{
    public Guid UserId { get; private set; }

    public Guid CourtId { get; private set; }

    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string? Phone { get; private set; }

    public StaffRole Role { get; private set; }
    public bool IsActive { get; private set; }

    public FamilyCourt Court { get; private set; } = null!;
    public ICollection<StaffWorkload> Workloads { get; private set; } = [];

    private CourtStaff() { }

    public static CourtStaff Create(
        Guid userId,
        Guid courtId,
        string email,
        string fullName,
        StaffRole role,
        string? phone = null)
    {
        return new CourtStaff
        {
            CourtId = courtId,
            UserId = userId,
            FullName = fullName,
            Email = email,
            Phone = phone,
            Role = role,
            IsActive = true
        };
    }

    public void IncrementLoad(AssignmentType type)
    {
        var workload = Workloads.FirstOrDefault(w => w.Type == type);
        if (workload is null)
        {
            workload = StaffWorkload.Create(Id, type);
            Workloads.Add(workload);
        }

        workload.Increment();
    }

    public void DecrementLoad(AssignmentType type)
    {
        var workload = Workloads.FirstOrDefault(w => w.Type == type);
        workload?.Decrement();
    }

    public void UpdateProfile(string fullName, string? phone)
    {
        FullName = fullName;
        Phone = phone;
    }
}