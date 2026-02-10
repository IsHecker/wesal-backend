using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

public record struct CreateVisitationScheduleCommand(
    Guid CourtId,
    Guid CourtCaseId,
    Guid LocationId,
    string Frequency,
    DateOnly StartDate,
    DateOnly? EndDate,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<Guid>;