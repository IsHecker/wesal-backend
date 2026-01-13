using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Domain.Entities.Families;

public sealed class Family : Entity
{
    public Guid FatherId { get; private set; }
    public Guid MotherId { get; private set; }

    public Parent Father { get; private set; } = null!;
    public Parent Mother { get; private set; } = null!;
    public ICollection<Child> Children { get; private set; } = [];
    public ICollection<CourtCase> CourtCases { get; private set; } = [];
    public ICollection<Visitation> Visitations { get; private set; } = [];

    private Family() { }

    public static Family Create(Guid fatherId, Guid motherId)
    {
        return new Family
        {
            FatherId = fatherId,
            MotherId = motherId,
        };
    }
}