using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.StaffPerformanceReports;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.StaffPerformanceReports.ListStaffReports;

internal sealed class ListStaffReportsQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListStaffReportsQuery, PagedResponse<StaffPerformanceSummaryResponse>>
{
    public async Task<Result<PagedResponse<StaffPerformanceSummaryResponse>>> Handle(
        ListStaffReportsQuery request,
        CancellationToken cancellationToken)
    {
        var courtStaffQuery = context.CourtStaffs
            .Include(s => s.Workloads)
            .Where(s => s.CourtId == request.CourtId && s.Role != StaffRole.Manager);

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var roleEnum = request.Role.ToEnum<StaffRole>();
            courtStaffQuery = courtStaffQuery.Where(s => s.Role == roleEnum);
        }

        var totalCount = await courtStaffQuery.CountAsync(cancellationToken);

        var staffList = await courtStaffQuery
            .OrderBy(s => s.FullName)
            .Paginate(request.Pagination)
            .ToListAsync(cancellationToken);

        var staffIds = staffList.Select(s => s.Id).ToList();

        // Batch queries for performance
        var families = await context.Families.AsNoTracking()
            .Where(f => f.AssignedStaffId != null && staffIds.Contains(f.AssignedStaffId.Value))
            .ToListAsync(cancellationToken);
            
        var cases = await context.CourtCases.AsNoTracking()
            .Where(c => staffIds.Contains(c.AssignedStaffId))
            .ToListAsync(cancellationToken);
            
        var complaints = await context.Complaints.AsNoTracking()
            .Where(c => staffIds.Contains(c.AssignedStaffId))
            .ToListAsync(cancellationToken);
            
        var alerts = await context.ObligationAlerts.AsNoTracking()
            .Where(a => staffIds.Contains(a.AssignedStaffId))
            .ToListAsync(cancellationToken);

        var responses = new List<StaffPerformanceSummaryResponse>();

        foreach (var staff in staffList)
        {
            int totalAssigned = 0;
            int totalResolved = 0;
            double totalDays = 0;
            int resolvedCountForAvg = 0;

            if (staff.Role == StaffRole.SettlementSpecialist)
            {
                var staffFamilies = families.Where(f => f.AssignedStaffId == staff.Id).ToList();
                totalAssigned = staffFamilies.Count;
                totalResolved = staffFamilies.Count(f => f.Status == FamilyStatus.Settled || f.Status == FamilyStatus.Escalated);

                foreach (var f in staffFamilies.Where(x => x.ResolvedAt.HasValue))
                {
                    totalDays += (f.ResolvedAt!.Value - f.CreatedAt).TotalDays;
                    resolvedCountForAvg++;
                }
            }
            else if (staff.Role == StaffRole.CaseClerk)
            {
                var staffCases = cases.Where(c => c.AssignedStaffId == staff.Id).ToList();
                totalAssigned = staffCases.Count;
                totalResolved = staffCases.Count(c => c.Status == CourtCaseStatus.Closed);

                foreach (var c in staffCases.Where(x => x.ClosedAt.HasValue))
                {
                    totalDays += (c.ClosedAt!.Value - c.FiledAt).TotalDays;
                    resolvedCountForAvg++;
                }
            }
            else if (staff.Role == StaffRole.ComplianceMonitor)
            {
                var staffComplaints = complaints.Where(c => c.AssignedStaffId == staff.Id).ToList();
                var staffAlerts = alerts.Where(a => a.AssignedStaffId == staff.Id).ToList();

                totalAssigned = staffComplaints.Count + staffAlerts.Count;
                totalResolved = staffComplaints.Count(c => c.ResolvedAt.HasValue) + staffAlerts.Count(a => a.ResolvedAt.HasValue);

                foreach (var c in staffComplaints.Where(x => x.ResolvedAt.HasValue))
                {
                    totalDays += (c.ResolvedAt!.Value - c.FiledAt).TotalDays;
                    resolvedCountForAvg++;
                }

                foreach (var a in staffAlerts.Where(x => x.ResolvedAt.HasValue))
                {
                    totalDays += (a.ResolvedAt!.Value - a.TriggeredAt).TotalDays;
                    resolvedCountForAvg++;
                }
            }

            double? avgDays = null;
            if (resolvedCountForAvg > 0)
                avgDays = Math.Round(totalDays / resolvedCountForAvg, 1);

            responses.Add(new StaffPerformanceSummaryResponse(
                staff.Id,
                staff.FullName,
                staff.Role.ToString(),
                totalAssigned,
                totalResolved,
                staff.Workloads.Sum(w => w.LoadCount),
                avgDays
            ));
        }

        return new PagedResponse<StaffPerformanceSummaryResponse>
        {
            Items = responses,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize,
            TotalCount = totalCount
        };
    }
}