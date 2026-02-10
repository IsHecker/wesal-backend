using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Custodies;

public sealed class Custody : Entity
{
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }

    public Guid CustodialParentId { get; private set; }
    public Guid NonCustodialParentId { get; private set; }

    public DateTime StartAt { get; private set; }
    public DateTime? EndAt { get; private set; }

    private Custody() { }

    public static Custody Create(
        Guid courtCaseId,
        Guid familyId,
        Guid custodialParentId,
        Guid nonCustodialParentId,
        DateTime startAt,
        DateTime? endAt = null)
    {
        return new Custody
        {
            CourtCaseId = courtCaseId,
            FamilyId = familyId,
            CustodialParentId = custodialParentId,
            NonCustodialParentId = nonCustodialParentId,
            StartAt = startAt,
            EndAt = endAt,
        };
    }

    public void Update(Guid newCustodialParentId, DateTime startAt, DateTime? endAt)
    {
        CustodialParentId = newCustodialParentId;
        StartAt = startAt;
        EndAt = endAt;
    }
}