namespace Wesal.Contracts.Complaints;

public record struct ComplaintResponse(
    Guid Id,
    Guid ReporterId,
    string Type,
    string Status,
    string Description,
    DateTime FiledAt,
    DateTime? ResolvedAt,
    string? ResolutionNotes);