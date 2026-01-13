using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Domain.Entities.Custodies;

public sealed class Custody : Entity
{
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }

    public Guid CustodianId { get; private set; }

    public DateTime StartAt { get; private set; }
    public DateTime? EndAt { get; private set; }

    public CourtCase CourtCase { get; private set; } = null!;
    public Child Child { get; private set; } = null!;
    public Parent Custodian { get; private set; } = null!;

    private Custody() { }

    public static Custody Create(
        Guid courtCaseId,
        Guid familyId,
        Guid custodianId,
        DateTime startAt,
        DateTime? endAt = null)
    {
        return new Custody
        {
            CourtCaseId = courtCaseId,
            FamilyId = familyId,
            CustodianId = custodianId,
            StartAt = startAt,
            EndAt = endAt,
        };
    }
}