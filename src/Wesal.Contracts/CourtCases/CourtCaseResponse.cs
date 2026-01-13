namespace Wesal.Contracts.CourtCases;

public record struct CourtCaseResponse(
    Guid Id,
    Guid CourtId,
    Guid FamilyId,
    string CaseNumber,
    DateTime FiledAt,
    string Status,
    string DecisionSummary);