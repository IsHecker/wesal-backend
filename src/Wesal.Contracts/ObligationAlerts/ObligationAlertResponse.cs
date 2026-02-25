namespace Wesal.Contracts.ObligationAlerts;

public record struct ObligationAlertResponse(
    Guid Id,
    Guid CourtId,
    Guid ParentId,
    Guid RelatedEntityId,
    string ParentName,
    string ViolationType,
    string Description,
    DateTime TriggeredAt,
    string Status,
    DateTime? ResolvedAt,
    string? ResolutionNotes);