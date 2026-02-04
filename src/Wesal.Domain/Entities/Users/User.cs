using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Users;

public sealed class User : Entity
{
    public string Role { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool PasswordChangeRequired { get; private set; }

    private User() { }

    public static User Create(string role, string password)
    {
        return new User
        {
            Role = role,
            PasswordHash = password,
            PasswordChangeRequired = true
        };
    }

    public void ChangePassword(string newPassword)
    {
        PasswordHash = newPassword;
        PasswordChangeRequired = false;
    }
}