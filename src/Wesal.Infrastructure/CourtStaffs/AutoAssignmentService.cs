using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Entities.CourtStaffs;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class AutoAssignmentService(IWesalDbContext context) : IAutoAssignmentService
{
    public async Task<CourtStaff> GetLowestLoadStaffAsync(
        Guid courtId,
        StaffRole role,
        AssignmentType assignmentType,
        CancellationToken cancellationToken = default)
    {
        var staff = await context.CourtStaffs
            .Include(s => s.Workloads)
            .AsTracking()
            .Where(s => s.CourtId == courtId && s.Role == role)
            .OrderBy(s => s.Workloads.Where(w => w.Type == assignmentType).Select(w => w.LoadCount).FirstOrDefault())
            .FirstOrDefaultAsync(cancellationToken);

        return staff ?? throw new InvalidOperationException(
            $"No staff with role '{role}' found in court '{courtId}'.");
    }

    public async Task<CourtStaff> GetBalancedComplianceMonitorAsync(
        Guid courtId,
        AssignmentType assignmentType,
        CancellationToken cancellationToken = default)
    {
        var staff = await context.CourtStaffs
            .AsTracking()
            .Include(s => s.Workloads)
            .Where(s => s.CourtId == courtId && s.Role == StaffRole.ComplianceMonitor)
            .OrderBy(s => s.Workloads.Where(w => w.Type == assignmentType).Select(w => w.LoadCount).FirstOrDefault())
            .ThenBy(s => s.Workloads.Sum(w => w.LoadCount))
            .FirstOrDefaultAsync(cancellationToken);

        return staff ?? throw new InvalidOperationException(
            $"No compliance monitors found in court '{courtId}'.");
    }
}