using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Domain.Entities.Users;

public sealed class User : Entity
{
    public string Role { get; private set; } = null!;

    public Parent ParentDetails { get; private set; } = null!;
    public FamilyCourt FamilyCourtDetails { get; private set; } = null!;
    public School SchoolDetails { get; private set; } = null!;
    public ICollection<Visitation> VerifiedVisitations { get; private set; } = [];

    private User() { }

    public static User Create(string role)
    {
        return new User
        {
            Role = role,
        };
    }
}