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

    public FamilyCourt Court { get; private set; } = null!;

    private CourtStaff() { }

    public static CourtStaff Create(
        Guid userId,
        Guid courtId,
        string email,
        string fullName,
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