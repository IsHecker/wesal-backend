namespace Wesal.Contracts.Visitations;

public record struct VisitationResponse(
    Guid Id,
    Guid FamilyId,
    Guid ParentId,
    Guid LocationId,
    Guid VisitationScheduleId,
    DateTime ScheduledVisitAt,
    DateTime? VisitedAt,
    string Status,
    bool IsCheckedIn,
    DateTime? CheckedInAt);