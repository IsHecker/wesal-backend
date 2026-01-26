using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Complaints;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.ListComplaintsByCourt;

internal sealed class ListComplaintsByCourtQueryHandler(
    ICourtStaffRepository staffRepository,
    IWesalDbContext context)
    : IQueryHandler<ListComplaintsByCourtQuery, PagedResponse<ComplaintResponse>>
{
    public async Task<Result<PagedResponse<ComplaintResponse>>> Handle(
        ListComplaintsByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var query = context.Complaints
            .Where(complaint => complaint.CourtId == staff.CourtId)
            .OrderByDescending(complaint => complaint.FiledAt);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(complaint => new ComplaintResponse(
                complaint.Id,
                complaint.ReporterId,
                complaint.Type.ToString(),
                complaint.Status.ToString(),
                complaint.Description,
                complaint.FiledAt,
                complaint.ResolvedAt,
                complaint.ResolutionNotes))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}