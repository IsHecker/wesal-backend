namespace Wesal.Contracts.VisitationLocations;

public record struct VisitationLocationResponse(
    Guid Id,
    string Name,
    string Address,
    string Governorate,
    string? ContactNumber,
    int MaxConcurrentVisits,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime);