using Wesal.Application.Data;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Application.Abstractions.Repositories;

public interface IVisitationRepository : IRepository<Visitation>
{
    Task<bool> HasCompletedByScheduleIdAsync(
        Guid scheduleId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByLocationIdAsync(
        Guid locationId,
        CancellationToken cancellationToken = default);

    Task DeleteScheduledByScheduleIdAsync(
        Guid scheduleId,
        CancellationToken cancellationToken = default);

    Task DeleteScheduledByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default);
}