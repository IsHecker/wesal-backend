namespace Wesal.Contracts.VisitationSchedules;

public record struct VisitationScheduleResponse(
    Guid Id,
    Guid CourtCaseId,
    Guid FamilyId,
    Guid ParentId,
    Guid LocationId,
    string Frequency,
    DateOnly StartDate,
    DateOnly? EndDate,
    TimeOnly StartTime,
    TimeOnly EndTime);