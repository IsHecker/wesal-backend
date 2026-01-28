namespace Wesal.Contracts.Visitations;

public record struct VisitationResponse(
    Guid Id,
    Guid FamilyId,
    Guid ParentId,
    Guid LocationId,
    Guid VisitationScheduleId,
    DateTime StartAt,
    DateTime EndAt,
    DateTime? CompletedAt,
    string Status,
    DateTime? CheckedInAt);