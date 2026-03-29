using Wesal.Application.Messaging;

namespace Wesal.Application.CustodyRequests.RespondToCustodyRequest;

public record struct RespondToCustodyRequestCommand(
    Guid RequestId,
    Guid ParentId,
    bool IsAccepted,
    string? ReasonNote) : ICommand;
