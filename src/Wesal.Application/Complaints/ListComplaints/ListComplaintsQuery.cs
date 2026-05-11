using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Complaints;

namespace Wesal.Application.Complaints.ListComplaints;

public record struct ListComplaintsQuery(
    Guid StaffId,
    string? Status,
    Pagination Pagination) : IQuery<ComplaintsResponse>;