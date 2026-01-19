using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.VisitCenterStaffs;

internal sealed class VisitCenterStaffRepository(WesalDbContext context)
    : Repository<VisitCenterStaff>(context), IVisitCenterStaffRepository
{
    public Task<VisitCenterStaff?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return context.VisitCenterStaffs
            .FirstOrDefaultAsync(school => school.UserId == userId, cancellationToken);
    }
}