using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.CheckOutVisitation;

public record struct CheckOutVisitationCommand(
    Guid StaffId,
    string NationalId,
    Guid VisitationId) : ICommand;