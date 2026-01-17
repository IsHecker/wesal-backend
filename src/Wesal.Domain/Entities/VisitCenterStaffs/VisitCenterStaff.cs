using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.VisitCenterStaffs;

public sealed class VisitCenterStaff : Entity
{
    public Guid UserId { get; private set; }

    public Guid LocationId { get; private set; }

    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string? Phone { get; private set; }

    private VisitCenterStaff() { }

    public static VisitCenterStaff Create(
        Guid userId,
        Guid locationId,
        string email,
        string fullName,
        string? phone = null)
    {
        return new VisitCenterStaff
        {
            LocationId = locationId,
            UserId = userId,
            FullName = fullName,
            Email = email,
            Phone = phone
        };
    }
}