using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Schools;

public sealed class School : Entity, IHasUserId
{
    public Guid UserId { get; private set; }
    public string Username { get; private set; } = null!;

    public string? Email { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string? ContactNumber { get; private set; }

    private School() { }

    public static School Create(
        Guid userId,
        string userName,
        string name,
        string address,
        string governorate,
        string? contactNumber = null)
    {
        return new School
        {
            UserId = userId,
            Username = userName,
            Name = name,
            Address = address,
            Governorate = governorate,
            ContactNumber = contactNumber,
        };
    }

    public void UpdateProfile(string? email, string? contactNumber)
    {
        Email = email;
        ContactNumber = contactNumber;
    }
}