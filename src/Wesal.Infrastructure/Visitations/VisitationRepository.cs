using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Visitations;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations;

internal sealed class VisitationRepository(WesalDbContext context)
    : Repository<Visitation>(context), IVisitationRepository
{
    public async Task DeleteScheduledByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default)
    {
        await context.Visitations
            .Where(visitation => visitation.VisitationSchedule.CourtCaseId == courtCaseId
                && visitation.Status == VisitationStatus.Scheduled).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteScheduledByScheduleIdAsync(
        Guid scheduleId,
        CancellationToken cancellationToken = default)
    {
        await context.Visitations
            .Where(visitation => visitation.VisitationScheduleId == scheduleId
                && visitation.Status == VisitationStatus.Scheduled).ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> ExistsByLocationIdAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        return context.Visitations
            .AnyAsync(visitation => visitation.LocationId == locationId, cancellationToken);
    }

    public Task<bool> HasCompletedByScheduleIdAsync(
        Guid scheduleId,
        CancellationToken cancellationToken = default)
    {
        return context.Visitations
            .AnyAsync(visitation => visitation.VisitationScheduleId == scheduleId
                && visitation.Status == VisitationStatus.Scheduled, cancellationToken);
    }
}