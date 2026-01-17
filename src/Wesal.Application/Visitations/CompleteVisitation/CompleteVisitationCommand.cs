using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.CompleteVisitation;

public record struct CompleteVisitationCommand(Guid VisitationId, Guid StaffId) : ICommand;