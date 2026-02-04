using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationSchedules.DeleteVisitationSchedule;

public record struct DeleteVisitationScheduleCommand(Guid CourtId, Guid ScheduleId) : ICommand;