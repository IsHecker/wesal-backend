using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.ChildReports;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Domain.Entities.Children;

public sealed class Child : Entity
{
    public Guid FamilyId { get; private set; }

    public Guid? SchoolId { get; private set; }

    public string FullName { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public string Gender { get; private set; } = null!;


    public Family Family { get; private set; } = null!;
    public School? School { get; private set; }
    public ICollection<ChildReport> ChildReports { get; private set; } = [];

    public Custody Custody { get; private set; } = null!;

    private Child() { }

    public static Child Create(
        Guid familyId,
        string fullName,
        DateTime birthDate,
        string gender,
        Guid? schoolId = null)
    {
        return new Child
        {
            FamilyId = familyId,
            SchoolId = schoolId,
            FullName = fullName,
            BirthDate = birthDate,
            Gender = gender,
        };
    }
}