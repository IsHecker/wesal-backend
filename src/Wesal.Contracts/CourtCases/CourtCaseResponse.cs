namespace Wesal.Contracts.CourtCases;

public record struct CourtCaseResponse(
    Guid Id,
    Guid CourtId,
    Guid FamilyId,
    Guid? DocumentId,
    string CaseNumber,
    DateTime FiledAt,
    string Status,
    string DecisionSummary,
    string? ClosureNotes,
    DateTime? ClosedAt);