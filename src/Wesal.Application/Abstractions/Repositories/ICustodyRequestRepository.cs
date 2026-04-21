using Wesal.Application.Data;
using Wesal.Domain.Entities.CustodyRequests;

namespace Wesal.Application.Abstractions.Repositories;

public interface ICustodyRequestRepository : IRepository<CustodyRequest>
{
    Task<bool> HasPendingByParentAndFamilyAsync(
        Guid parentId,
        Guid familyId,
        CancellationToken cancellationToken = default);
}