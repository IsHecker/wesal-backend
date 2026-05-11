using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtStaffs;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtStaffs.ListCourtStaffsByCourt;

internal sealed class ListCourtStaffsByCourtQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListCourtStaffsByCourtQuery, PagedResponse<CourtStaffResponse>>
{
    public async Task<Result<PagedResponse<CourtStaffResponse>>> Handle(
        ListCourtStaffsByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.CourtStaffs
            .Include(staff => staff.Workloads)
            .Where(staff => staff.CourtId == request.CourtId);

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var roleEnum = request.Role.ToEnum<StaffRole>();
            query = query.Where(staff => staff.Role == roleEnum);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .OrderBy(staff => staff.FullName)
            .Paginate(request.Pagination)
            .Select(staff => new CourtStaffResponse(
                staff.Id,
                staff.CourtId,
                staff.FullName,
                staff.Email,
                staff.Phone,
                staff.Role.ToString(),
                staff.IsActive))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}