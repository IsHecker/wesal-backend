using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

public record struct CreateVisitationScheduleCommand(
    Guid CourtCaseId,
    Guid ParentId,
    Guid LocationId,
    string Frequency,
    DateOnly StartDate,
    DateOnly EndDate,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<Guid>;