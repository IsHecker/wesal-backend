using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CustodyRequests;
using Wesal.Domain.Entities.Users;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

public record struct ListCustodyRequestsQuery(
    Guid UserId,
    UserRole UserRole,
    string? Status,
    Pagination Pagination) : IQuery<PagedResponse<CustodyRequestResponse>>;