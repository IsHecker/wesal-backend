using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationSchedules.UpdateVisitationSchedule;

public record struct UpdateVisitationScheduleCommand(
    Guid CourtId,
    Guid ScheduleId,
    Guid LocationId,
    string Frequency,
    TimeOnly StartTime,
    TimeOnly EndTime,
    DateOnly StartDate,
    DateOnly? EndDate) : ICommand;