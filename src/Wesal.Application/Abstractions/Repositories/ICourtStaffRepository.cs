using Wesal.Application.Data;
using Wesal.Domain.Entities.CourtStaffs;

namespace Wesal.Application.Abstractions.Repositories;

public interface ICourtStaffRepository : IRepository<CourtStaff>
{
    Task<CourtStaff?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CourtStaff?> GetByUserIdWithCourtAsync(Guid userId, CancellationToken cancellationToken);
}