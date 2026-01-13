using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Domain.Entities.CourtCases;

public sealed class CourtCase : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FamilyId { get; private set; }

    public string CaseNumber { get; private set; }
    public DateTime FiledAt { get; private set; }
    public CourtCaseStatus Status { get; private set; }
    public string DecisionSummary { get; private set; } = null!;

    public FamilyCourt Court { get; private set; } = null!;
    public Family Family { get; private set; } = null!;
    public Custody Custody { get; private set; } = null!;
    public ICollection<VisitationSchedule> VisitationSchedules { get; private set; } = [];
    public ICollection<Alimony> Alimonies { get; private set; } = [];
    public ICollection<ObligationAlert> ObligationAlerts { get; private set; } = [];

    private CourtCase() { }

    public static CourtCase Create(
        Guid courtId,
        Guid familyId,
        string caseNumber,
        DateTime filedAt,
        CourtCaseStatus status,
        string decisionSummary)
    {
        return new CourtCase
        {
            CourtId = courtId,
            FamilyId = familyId,
            CaseNumber = caseNumber,
            FiledAt = filedAt,
            Status = status,
            DecisionSummary = decisionSummary,
        };
    }
}