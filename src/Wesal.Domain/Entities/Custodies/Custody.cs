using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Custodies;

public sealed class Custody : Entity
{
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }

    public Guid CustodianId { get; private set; }

    public DateTime StartAt { get; private set; }
    public DateTime? EndAt { get; private set; }

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

    public void Update(Guid newCustodianId, DateTime startAt, DateTime? endAt)
    {
        CustodianId = newCustodianId;
        StartAt = startAt;
        EndAt = endAt;
    }
}