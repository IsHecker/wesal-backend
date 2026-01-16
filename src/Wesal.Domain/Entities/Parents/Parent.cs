using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Parents;

public sealed class Parent : Entity
{
    public Guid UserId { get; private set; }

    public string? Email { get; private set; }

    public string NationalId { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public string Gender { get; private set; } = null!;
    public string? Job { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }

    private Parent() { }

    public static Parent Create(
        string nationalId,
        string fullName,
        DateTime birthDate,
        string gender,
        string? job = null,
        string? address = null,
        string? phone = null,
        string? email = null)
    {
        return new Parent
        {
            NationalId = nationalId,
            FullName = fullName,
            BirthDate = birthDate,
            Gender = gender,
            Job = job,
            Address = address,
            Phone = phone,
            Email = email
        };
    }
}