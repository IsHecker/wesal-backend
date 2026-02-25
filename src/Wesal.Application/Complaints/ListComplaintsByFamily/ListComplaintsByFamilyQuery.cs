using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Complaints;

namespace Wesal.Application.Complaints.ListComplaintsByFamily;

public record struct ListComplaintsByFamilyQuery(Guid FamilyId, Pagination Pagination)
    : IQuery<PagedResponse<ComplaintResponse>>;