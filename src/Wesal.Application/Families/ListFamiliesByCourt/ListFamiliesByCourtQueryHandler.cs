using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.ListFamiliesByCourt;

internal sealed class ListFamiliesByCourtQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListFamiliesByCourtQuery, PagedResponse<FamilyResponse>>
{
    public async Task<Result<PagedResponse<FamilyResponse>>> Handle(
        ListFamiliesByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Families
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .Where(family => family.CourtId == request.CourtId);

        var totalCount = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.NationalId))
            query = query.Where(family => family.Father.NationalId == request.NationalId
                || family.Mother.NationalId == request.NationalId);

        return await query
            .OrderByDescending(family => family.CreatedAt)
            .Paginate(request.Pagination)
            .Select(family => family.ToResponse())
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}