using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitCenterStaffs.ListVisitCenterStaffsByCourt;

internal sealed class ListVisitCenterStaffsByCourtQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListVisitCenterStaffsByCourtQuery, PagedResponse<VisitCenterStaffResponse>>
{
    public async Task<Result<PagedResponse<VisitCenterStaffResponse>>> Handle(
        ListVisitCenterStaffsByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.VisitCenterStaffs
            .Where(staff => staff.CourtId == request.CourtId);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedStaffs = await query
            .OrderBy(staff => staff.FullName)
            .Paginate(request.Pagination)
            .Select(staff => new VisitCenterStaffResponse(
                staff.Id,
                staff.CourtId,
                staff.LocationId,
                staff.FullName,
                staff.Email,
                staff.Phone))
            .ToPagedResponseAsync(request.Pagination, totalCount);

        return pagedStaffs;
    }
}