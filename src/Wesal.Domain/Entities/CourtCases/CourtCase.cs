using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.CourtCases;

public sealed class CourtCase : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FamilyId { get; private set; }

    public string CaseNumber { get; private set; } = null!;
    public DateTime FiledAt { get; private set; }
    public CourtCaseStatus Status { get; private set; }
    public string DecisionSummary { get; private set; } = null!;

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