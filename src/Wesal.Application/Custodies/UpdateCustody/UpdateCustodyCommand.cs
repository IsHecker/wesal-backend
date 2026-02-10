using Wesal.Application.Messaging;

namespace Wesal.Application.Custodies.UpdateCustody;

public record struct UpdateCustodyCommand(
    Guid CourtId,
    Guid CustodyId,
    Guid NewCustodialParentId,
    DateTime StartAt,
    DateTime? EndAt) : ICommand;