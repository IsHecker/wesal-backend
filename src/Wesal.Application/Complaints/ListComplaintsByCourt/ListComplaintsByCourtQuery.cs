using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Complaints;

namespace Wesal.Application.Complaints.ListComplaintsByCourt;

public record struct ListComplaintsByCourtQuery(
    Guid CourtId,
    string? Status,
    Pagination Pagination) : IQuery<ComplaintsResponse>;