namespace Wesal.Contracts.CustodyRequests;

public record struct CustodyRequestResponse(
    Guid Id,
    Guid FamilyId,
    Guid CourtCaseId,
    Guid CustodyId,
    string FatherName,
    string MotherName,
    DateOnly StartDate,
    DateOnly EndDate,
    string Reason,
    string Status,
    DateTime RequestedAt,
    string? DecisionNote,
    DateTime? ProcessedAt);