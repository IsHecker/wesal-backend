using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.StaffPerformanceReports;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.StaffPerformanceReports.GetStaffReport;

internal sealed class GetStaffReportQueryHandler(IWesalDbContext context)
    : IQueryHandler<GetStaffReportQuery, StaffPerformanceReportResponse>
{
    public async Task<Result<StaffPerformanceReportResponse>> Handle(
        GetStaffReportQuery request,
        CancellationToken cancellationToken)
    {
        var staff = await context.CourtStaffs
            .Include(s => s.Workloads)
            .FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken);

        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var report = new StaffPerformanceReportResponse
        {
            StaffId = staff.Id,
            FullName = staff.FullName,
            Role = staff.Role.ToString(),
            CurrentlyOpenItems = staff.Workloads.Sum(w => w.LoadCount)
        };

        if (staff.Role == StaffRole.SettlementSpecialist)
        {
            var families = await context.Families
                .Where(f => f.AssignedStaffId == staff.Id)
                .ToListAsync(cancellationToken);

            report.TotalFamiliesEnrolled = families.Count;
            report.SuccessfulSettlements = families.Count(f => f.Status == FamilyStatus.Settled);
            report.EscalatedFamilies = families.Count(f => f.Status == FamilyStatus.Escalated);

            var resolvedFamilies = families.Where(f => f.ResolvedAt.HasValue);
            if (resolvedFamilies.Any())
            {
                report.AverageResolutionTimeDays = Math.Round(
                    resolvedFamilies.Average(f => (f.ResolvedAt!.Value - f.CreatedAt).TotalDays), 1);
            }
        }
        else if (staff.Role == StaffRole.CaseClerk)
        {
            var cases = await context.CourtCases
                .Where(c => c.AssignedStaffId == staff.Id)
                .ToListAsync(cancellationToken);

            report.TotalCasesAssigned = cases.Count;
            report.CasesClosed = cases.Count(c => c.Status == CourtCaseStatus.Closed);

            var closedCases = cases.Where(c => c.ClosedAt.HasValue);
            if (closedCases.Any())
            {
                report.AverageResolutionTimeDays = Math.Round(
                    closedCases.Average(c => (c.ClosedAt!.Value - c.FiledAt).TotalDays), 1);
            }
        }
        else if (staff.Role == StaffRole.ComplianceMonitor)
        {
            var complaints = await context.Complaints
                .Where(c => c.AssignedStaffId == staff.Id)
                .ToListAsync(cancellationToken);

            var alerts = await context.ObligationAlerts
                .Where(a => a.AssignedStaffId == staff.Id)
                .ToListAsync(cancellationToken);

            report.TotalComplaintsAssigned = complaints.Count;
            report.ComplaintsResolved = complaints.Count(c => c.ResolvedAt.HasValue);

            report.TotalAlertsAssigned = alerts.Count;
            report.AlertsResolved = alerts.Count(a => a.ResolvedAt.HasValue);

            // Refiled Complaints: Multiple complaints for same family
            // We'll approximate this by finding complaints for families that already had a previous complaint
            var familyGroups = complaints.GroupBy(c => c.FamilyId);
            report.RefiledComplaints = familyGroups.Where(g => g.Count() > 1).Sum(g => g.Count() - 1);

            double complaintTotalDays = 0;
            int complaintCount = 0;

            foreach (var c in complaints.Where(x => x.ResolvedAt.HasValue))
            {
                var days = (c.ResolvedAt!.Value - c.FiledAt).TotalDays;
                complaintTotalDays += days;
                complaintCount++;
            }

            double alertTotalDays = 0;
            int alertCount = 0;

            foreach (var a in alerts.Where(x => x.ResolvedAt.HasValue))
            {
                var days = (a.ResolvedAt!.Value - a.TriggeredAt).TotalDays;
                alertTotalDays += days;
                alertCount++;
            }

            var count = complaintCount + alertCount;

            if (count > 0)
            {
                report.AverageResolutionTimeDays = Math.Round(complaintTotalDays + alertTotalDays / count, 1);
            }
            if (complaintCount > 0)
            {
                report.AverageComplaintResolutionTimeDays = Math.Round(complaintTotalDays / complaintCount, 1);
            }
            if (alertCount > 0)
            {
                report.AverageAlertResolutionTimeDays = Math.Round(alertTotalDays / alertCount, 1);
            }
        }

        return report;
    }
}