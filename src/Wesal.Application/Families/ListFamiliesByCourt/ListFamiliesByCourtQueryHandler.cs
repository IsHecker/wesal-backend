using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.ListFamiliesByCourt;

internal sealed class ListFamiliesByCourtQueryHandler(
    ICourtStaffRepository courtStaffRepository,
    IWesalDbContext context)
    : IQueryHandler<ListFamiliesByCourtQuery, PagedResponse<FamilyResponse>>
{
    public async Task<Result<PagedResponse<FamilyResponse>>> Handle(
        ListFamiliesByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var staff = await courtStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var query = context.Families
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .Where(family => family.CourtId == staff.CourtId)
            .OrderByDescending(family => family.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(family => family.ToResponse())
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}