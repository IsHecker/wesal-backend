using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.FamilyCourts;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.FamilyCourts.ListFamilyCourts;

internal sealed class ListFamilyCourtsQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListFamilyCourtsQuery, PagedResponse<FamilyCourtResponse>>
{
    public async Task<Result<PagedResponse<FamilyCourtResponse>>> Handle(
        ListFamilyCourtsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.FamilyCourts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(c => c.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Governorate))
            query = query.Where(c => c.Governorate == request.Governorate);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .OrderBy(c => c.Name)
            .Paginate(request.Pagination)
            .Select(court => new FamilyCourtResponse(
                court.Id,
                court.Email,
                court.Name,
                court.Governorate,
                court.Address,
                court.ContactInfo))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}