using Wesal.Contracts.Common;

namespace Wesal.Contracts.Complaints;


public record struct ComplaintsResponse(
    PagedResponse<ComplaintResponse> Complaints,
    int PendingCount,
    int UnderReviewCount,
    int ResolvedCount);