using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationLocations;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationLocations.ListVisitationLocations;

internal sealed class ListVisitationLocationsQueryHandler(
    IWesalDbContext context)
    : IQueryHandler<ListVisitationLocationsQuery, PagedResponse<VisitationLocationResponse>>
{
    public async Task<Result<PagedResponse<VisitationLocationResponse>>> Handle(
        ListVisitationLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = BuildQuery(request.Name, request.CourtId);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(location => new VisitationLocationResponse(
                location.Id,
                location.Name,
                location.Address,
                location.Governorate,
                location.ContactNumber,
                location.MaxConcurrentVisits,
                location.OpeningTime,
                location.ClosingTime))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }

    private IQueryable<VisitationLocation> BuildQuery(string? name, Guid courtId)
    {
        var query = context.VisitationLocations
            .Where(location => location.CourtId == courtId);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(location => location.Name.Contains(name));

        return query.OrderBy(s => s.Name);
    }
}