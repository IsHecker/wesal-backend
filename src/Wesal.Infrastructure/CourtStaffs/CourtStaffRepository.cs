using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class CourtStaffRepository(WesalDbContext context)
    : Repository<CourtStaff>(context), ICourtStaffRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.CourtStaffs
            .AnyAsync(staff => staff.Email == email, cancellationToken);
    }

    public Task<CourtStaff?> GetByIdWithWorkloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.CourtStaffs
            .Include(staff => staff.Workloads)
            .AsTracking()
            .FirstOrDefaultAsync(staff => staff.Id == id, cancellationToken);
    }

    public Task<CourtStaff?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.CourtStaffs
            .FirstOrDefaultAsync(staff => staff.UserId == userId, cancellationToken);
    }

    public Task<CourtStaff?> GetByUserIdWithCourtAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.CourtStaffs
            .Include(staff => staff.Court)
            .FirstOrDefaultAsync(staff => staff.UserId == userId, cancellationToken);
    }
}