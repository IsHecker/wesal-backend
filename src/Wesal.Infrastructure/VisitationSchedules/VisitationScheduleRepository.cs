using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.VisitationSchedules;

internal sealed class VisitationScheduleRepository(WesalDbContext context)
    : Repository<VisitationSchedule>(context), IVisitationScheduleRepository
{
    public Task<bool> ExistsByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default)
    {
        return context.VisitationSchedules
            .AnyAsync(schedule => schedule.CourtCaseId == courtCaseId, cancellationToken);
    }

    public Task<VisitationSchedule> GetByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default)
    {
        return context.VisitationSchedules
            .FirstAsync(schedule => schedule.CourtCaseId == courtCaseId, cancellationToken);
    }
}