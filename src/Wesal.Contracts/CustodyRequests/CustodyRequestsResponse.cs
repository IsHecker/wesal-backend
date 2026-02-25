using Wesal.Contracts.Common;

namespace Wesal.Contracts.CustodyRequests;

public record struct CustodyRequestsResponse(
    PagedResponse<CustodyRequestResponse> Requests,
    int PendingCount,
    int ApprovedCount,
    int RejectedCount);