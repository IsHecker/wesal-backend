using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Complaints;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Complaints;

internal sealed class ComplaintRepository(WesalDbContext context)
    : Repository<Complaint>(context), IComplaintRepository
{
    public Task<int> GetMonthCountByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return context.Complaints
            .CountAsync(complaint => complaint.FiledAt.Month == EgyptTime.Now.Month, cancellationToken);
    }
}