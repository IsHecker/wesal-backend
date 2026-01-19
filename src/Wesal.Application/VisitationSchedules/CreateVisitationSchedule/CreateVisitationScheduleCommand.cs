using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

public record struct CreateVisitationScheduleCommand(
    Guid UserId,
    Guid CourtCaseId,
    Guid ParentId,
    Guid LocationId,
    int StartDayInMonth,
    string Frequency,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<Guid>;