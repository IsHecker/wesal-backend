using Wesal.Application.Data;
using Wesal.Domain.Entities.Complaints;

namespace Wesal.Application.Abstractions.Repositories;

public interface IComplaintRepository : IRepository<Complaint>
{
    Task<int> GetMonthCountByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default);
}