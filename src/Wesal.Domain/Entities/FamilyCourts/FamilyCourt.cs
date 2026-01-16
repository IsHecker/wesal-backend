using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.FamilyCourts;

public sealed class FamilyCourt : Entity
{
    public Guid UserId { get; private set; }

    public string Email { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string ContactInfo { get; private set; } = null!;

    private FamilyCourt() { }

    public static FamilyCourt Create(
        string name,
        string governorate,
        string address,
        string contactInfo)
    {
        return new FamilyCourt
        {
            Name = name,
            Governorate = governorate,
            Address = address,
            ContactInfo = contactInfo,
        };
    }
}