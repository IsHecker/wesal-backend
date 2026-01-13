namespace Wesal.Contracts.Custodies;

public record struct CustodyResponse(
    Guid Id,
    Guid CourtCaseId,
    Guid FamilyId,
    Guid CustodianId,
    DateTime StartAt,
    DateTime? EndAt);