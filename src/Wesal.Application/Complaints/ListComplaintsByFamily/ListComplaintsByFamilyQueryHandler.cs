using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Complaints;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.ListComplaintsByFamily;

internal sealed class ListComplaintsByFamilyQueryHandler(
    IWesalDbContext context)
    : IQueryHandler<ListComplaintsByFamilyQuery, PagedResponse<ComplaintResponse>>
{
    public async Task<Result<PagedResponse<ComplaintResponse>>> Handle(
        ListComplaintsByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Complaints
            .Where(complaint => complaint.FamilyId == request.FamilyId);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Include(complaint => complaint.Reporter)
            .OrderByDescending(complaint => complaint.FiledAt)
            .Paginate(request.Pagination)
            .Select(complaint => new ComplaintResponse(
                complaint.Id,
                complaint.CourtId,
                complaint.FamilyId,
                complaint.ReporterId,
                complaint.DocumentId,
                complaint.Reporter.FullName,
                complaint.Type.ToString(),
                complaint.Status.ToString(),
                complaint.Description,
                complaint.FiledAt,
                complaint.ResolvedAt,
                complaint.ResolutionNotes))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}