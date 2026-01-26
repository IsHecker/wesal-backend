using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CustodyRequests;

public static class CustodyRequestErrors
{
    public static Error NoCustodyDecision =>
        Error.NotFound("CustodyRequest.NoCustodyDecision", "No custody decision exists for this family");

    public static Error AlreadyCustodian =>
        Error.Conflict("CustodyRequest.AlreadyCustodian", "You are already the custodial parent");

    public static Error NotFound(Guid requestId) =>
        Error.NotFound("CustodyRequest.NotFound", $"Custody request with ID '{requestId}' not found");
}