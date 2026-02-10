using Wesal.Application.Data;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Application.Abstractions.Repositories;

public interface IVisitationScheduleRepository : IRepository<VisitationSchedule>
{
    Task<bool> ExistsByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default);

    Task<VisitationSchedule> GetByCourtCaseIdAsync(Guid courtCaseId, CancellationToken cancellationToken = default);
}