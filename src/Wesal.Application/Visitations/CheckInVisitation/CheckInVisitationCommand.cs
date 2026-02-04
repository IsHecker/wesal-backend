using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.CheckInVisitation;

public record struct CheckInVisitationCommand(Guid CenterStaffId, Guid VisitationId)
    : ICommand;