using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class CourtStaffRepository(WesalDbContext context)
    : Repository<CourtStaff>(context), ICourtStaffRepository
{
}