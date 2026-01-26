using Wesal.Application.Messaging;

namespace Wesal.Application.CustodyRequests.ProcessCustodyRequest;

public record struct ProcessCustodyRequestCommand(
    Guid StaffId,
    Guid RequestId,
    bool IsApproved,
    string DecisionNote) : ICommand;