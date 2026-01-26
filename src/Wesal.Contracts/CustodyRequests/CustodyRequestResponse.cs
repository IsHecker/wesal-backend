namespace Wesal.Contracts.CustodyRequests;

public record struct CustodyRequestResponse(
    Guid Id,
    string FatherName,
    string MotherName,
    DateOnly StartDate,
    DateOnly EndDate,
    string Reason,
    string Status,
    DateTime RequestedAt,
    string? DecisionNote,
    DateTime? ProcessedAt);