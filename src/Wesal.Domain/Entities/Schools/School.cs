using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.ChildReports;
using Wesal.Domain.Entities.Users;

namespace Wesal.Domain.Entities.Schools;

public sealed class School : Entity
{
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string? ContactNumber { get; private set; }

    public User User { get; private set; } = null!;
    public ICollection<Child> Children { get; private set; } = [];
    public ICollection<ChildReport> ChildReportsIssued { get; private set; } = [];

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