using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.CustodyRequests;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

public record struct ListCustodyRequestsQuery(
    Guid UserId,
    Guid CourtId,
    string UserRole,
    Guid? FamilyId,
    string? Status,
    Pagination Pagination) : IQuery<CustodyRequestsResponse>;