using Wesal.Domain.Entities.CourtStaffs;

namespace Wesal.Application.Abstractions.Services;

public interface IAutoAssignmentService
{
    Task<CourtStaff> GetLowestLoadStaffAsync(Guid courtId, StaffRole role, AssignmentType type, CancellationToken cancellationToken = default);
    Task<CourtStaff> GetBalancedComplianceMonitorAsync(Guid courtId, AssignmentType assignmentType, CancellationToken cancellationToken = default);
}