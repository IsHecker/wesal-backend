using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Users;

public sealed class User : Entity
{
    public string Role { get; private set; } = null!;

    private User() { }

    public static User Create(string role)
    {
        return new User
        {
            Role = role,
        };
    }
}