using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.CheckInVisitation;

public record struct CheckInVisitationCommand(Guid VisitationId, Guid UserId)
    : ICommand;