namespace Wesal.Contracts.Complaints;

public record struct ComplaintResponse(
    Guid Id,
    Guid CourtId,
    Guid FamilyId,
    Guid ReporterId,
    Guid? DocumentId,
    string ReporterName,
    string Type,
    string Status,
    string Description,
    DateTime FiledAt,
    DateTime? ResolvedAt,
    string? ResolutionNotes);