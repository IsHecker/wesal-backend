using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.CourtStaffs;

public sealed class CourtStaff : Entity
{
    public Guid UserId { get; private set; }

    public Guid CourtId { get; private set; }

    public string FullName { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    private CourtStaff() { }

    public static CourtStaff Create(
        Guid userId,
        Guid courtId,
        string fullName,
        string? email = null,
        string? phone = null)
    {
        return new CourtStaff
        {
            CourtId = courtId,
            UserId = userId,
            FullName = fullName,
            Email = email,
            Phone = phone
        };
    }
}