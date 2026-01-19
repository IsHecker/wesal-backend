using Wesal.Application.Data;
using Wesal.Domain.Entities.VisitCenterStaffs;

namespace Wesal.Application.Abstractions.Repositories;

public interface IVisitCenterStaffRepository : IRepository<VisitCenterStaff>
{

    Task<VisitCenterStaff?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}