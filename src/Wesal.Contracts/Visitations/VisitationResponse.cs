namespace Wesal.Contracts.Visitations;

public record struct VisitationResponse(
    Guid Id,
    Guid FamilyId,
    Guid ParentId,
    Guid LocationId,
    Guid VisitationScheduleId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    DateTime? CompletedAt,
    string Status,
    DateTime? CheckedInAt);