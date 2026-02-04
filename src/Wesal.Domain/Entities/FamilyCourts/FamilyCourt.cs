using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.FamilyCourts;

public sealed class FamilyCourt : Entity, IHasUserId
{
    public Guid UserId { get; private set; }

    public string Email { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string ContactInfo { get; private set; } = null!;

    private FamilyCourt() { }

    public static FamilyCourt Create(
        Guid userId,
        string email,
        string name,
        string governorate,
        string address,
        string contactInfo)
    {
        return new FamilyCourt
        {
            UserId = userId,
            Email = email,
            Name = name,
            Governorate = governorate,
            Address = address,
            ContactInfo = contactInfo,
        };
    }
}