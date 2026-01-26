using Wesal.Application.Messaging;

namespace Wesal.Application.CustodyRequests.CreateCustodyRequest;

public record struct CreateCustodyRequestCommand(
    Guid ParentId,
    DateOnly StartDate,
    DateOnly EndDate,
    string Reason) : ICommand<Guid>;