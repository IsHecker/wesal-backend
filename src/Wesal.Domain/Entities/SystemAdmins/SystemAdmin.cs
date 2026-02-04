using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.SystemAdmins;

public sealed class SystemAdmin : Entity, IHasUserId
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;

    private SystemAdmin() { }

    public static SystemAdmin Create(
        Guid userId,
        string email,
        string fullName)
    {
        return new SystemAdmin
        {
            UserId = userId,
            Email = email,
            FullName = fullName
        };
    }
}