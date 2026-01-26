namespace Wesal.Contracts.VisitationSchedules;

public record struct VisitationScheduleResponse(
    Guid Id,
    Guid CourtCaseId,
    Guid FamilyId,
    Guid ParentId,
    Guid LocationId,
    int StartDayInMonth,
    string Frequency,
    TimeOnly StartTime,
    TimeOnly EndTime);