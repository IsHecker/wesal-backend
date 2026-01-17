using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationLocations;

namespace Wesal.Application.VisitationLocations.ListVisitationLocations;

public record struct ListVisitationLocationsQuery(
    Guid StaffId,
    string? Name,
    Pagination Pagination) : IQuery<PagedResponse<VisitationLocationResponse>>;