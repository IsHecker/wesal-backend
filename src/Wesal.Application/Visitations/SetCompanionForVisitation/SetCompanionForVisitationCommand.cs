using Wesal.Application.Messaging;

namespace Wesal.Application.Visitations.SetCompanionForVisitation;

public record struct SetCompanionForVisitationCommand(
    Guid CustodialParentId,
    Guid VisitationId,
    string CompanionNationalId) : ICommand;