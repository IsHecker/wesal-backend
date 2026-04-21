using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.CustodyRequests;

internal sealed class CustodyRequestRepository(WesalDbContext context)
    : Repository<CustodyRequest>(context), ICustodyRequestRepository
{
    public Task<bool> HasPendingByParentAndFamilyAsync(
        Guid parentId,
        Guid familyId,
        CancellationToken cancellationToken = default)
    {
        return context.Set<CustodyRequest>()
            .AnyAsync(r => r.FamilyId == familyId 
                && r.NonCustodialParentId == parentId 
                && r.Status == CustodyRequestStatus.Pending, cancellationToken);
    }
}