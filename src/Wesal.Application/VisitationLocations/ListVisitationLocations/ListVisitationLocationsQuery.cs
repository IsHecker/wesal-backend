using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationLocations;

namespace Wesal.Application.VisitationLocations.GetVisitationLocation;

public record struct ListVisitationLocationsQuery(
    Guid CourtId,
    string? Name,
    Pagination Pagination) : IQuery<PagedResponse<VisitationLocationResponse>>;