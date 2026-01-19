using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class CourtStaffRepository(WesalDbContext context)
    : Repository<CourtStaff>(context), ICourtStaffRepository
{
    public Task<CourtStaff?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return context.CourtStaffs
            .FirstOrDefaultAsync(staff => staff.UserId == userId, cancellationToken);
    }

    public Task<CourtStaff?> GetByUserIdWithCourtAsync(Guid userId, CancellationToken cancellationToken)
    {
        return context.CourtStaffs
            .Include(staff => staff.Court)
            .FirstOrDefaultAsync(staff => staff.UserId == userId, cancellationToken);
    }
}