using Wesal.Application.Messaging;

namespace Wesal.Application.Custodies.UpdateCustody;

public record struct UpdateCustodyCommand(
    Guid CourtId,
    Guid CustodyId,
    Guid NewCustodianId,
    DateTime StartAt,
    DateTime? EndAt) : ICommand;