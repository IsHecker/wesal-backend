using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Complaints;

namespace Wesal.Application.Complaints.ListComplaintsByCourt;

public record struct ListComplaintsByCourtQuery(Guid StaffId, Pagination Pagination)
    : IQuery<PagedResponse<ComplaintResponse>>;