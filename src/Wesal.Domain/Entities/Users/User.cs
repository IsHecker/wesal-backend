using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Users;

public sealed class User : Entity
{
    public UserRole Role { get; private set; }
    public string Username { get; private set; } = null!;
    public string? Password { get; private set; } = null!;

    private User() { }

    public static User Create(UserRole role, string userName, string? password = null)
    {
        return new User
        {
            Role = role,
            Username = userName,
            Password = password
        };
    }
}