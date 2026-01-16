using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Schools;

public sealed class School : Entity
{
    public Guid UserId { get; private set; }

    public string Email { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string? ContactNumber { get; private set; }

    private School() { }

    public static School Create(
        string name,
        string address,
        string governorate,
        string? contactNumber = null)
    {
        return new School
        {
            Name = name,
            Address = address,
            Governorate = governorate,
            ContactNumber = contactNumber,
        };
    }
}