using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.CheckInVisitation;

public record struct CheckInVisitationCommand(Guid CenterStaffId, string NationalId, Guid VisitationId)
    : ICommand;