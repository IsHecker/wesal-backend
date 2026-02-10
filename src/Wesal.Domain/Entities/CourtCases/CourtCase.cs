using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CourtCases;

public sealed class CourtCase : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid? DocumentId { get; private set; }

    public string CaseNumber { get; private set; } = null!;
    public DateTime FiledAt { get; private set; }
    public CourtCaseStatus Status { get; private set; }
    public string DecisionSummary { get; private set; } = null!;
    public string? ClosureNotes { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    private CourtCase() { }

    public static CourtCase Create(
        Guid courtId,
        Guid familyId,
        string caseNumber,
        string decisionSummary,
        Guid? documentId = null)
    {
        return new CourtCase
        {
            CourtId = courtId,
            FamilyId = familyId,
            DocumentId = documentId,
            CaseNumber = caseNumber,
            Status = CourtCaseStatus.Open,
            DecisionSummary = decisionSummary,
            FiledAt = EgyptTime.Now,
        };
    }

    public Result Close(string closureNotes)
    {
        if (Status == CourtCaseStatus.Closed)
            return CourtCaseErrors.AlreadyClosed;

        Status = CourtCaseStatus.Closed;
        ClosureNotes = closureNotes;
        ClosedAt = EgyptTime.Now;

        return Result.Success;
    }
}