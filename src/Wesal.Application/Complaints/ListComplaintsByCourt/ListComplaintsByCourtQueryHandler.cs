using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Complaints;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.ListComplaintsByCourt;

internal sealed class ListComplaintsByCourtQueryHandler(
    IWesalDbContext context)
    : IQueryHandler<ListComplaintsByCourtQuery, ComplaintsResponse>
{
    public async Task<Result<ComplaintsResponse>> Handle(
        ListComplaintsByCourtQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Complaints
            .Where(complaint => complaint.CourtId == request.CourtId);

        var pendingCount = await query.CountAsync(
            complaint => complaint.Status == ComplaintStatus.Pending,
            cancellationToken);

        var underReviewCount = await query.CountAsync(
            complaint => complaint.Status == ComplaintStatus.UnderReview,
            cancellationToken);

        var resolvedCount = await query.CountAsync(
            complaint => complaint.Status == ComplaintStatus.Resolved,
            cancellationToken);

        query = ApplyStatusFilter(query, request.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedComplaints = await query
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

        return new ComplaintsResponse(
            pagedComplaints,
            pendingCount,
            underReviewCount,
            resolvedCount);
    }

    private static IQueryable<Complaint> ApplyStatusFilter(
        IQueryable<Complaint> query,
        string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return query;

        var statusEnum = status.ToEnum<ComplaintStatus>();
        return query.Where(cr => cr.Status == statusEnum);
    }
}