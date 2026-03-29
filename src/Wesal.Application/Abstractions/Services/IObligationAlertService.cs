using Wesal.Domain.Entities.ObligationAlerts;

namespace Wesal.Application.Abstractions.Services;

public interface IObligationAlertService
{
    Task RecordViolationAsync(
        Guid parentId,
        ViolationType type,
        Guid sourceId,
        string violationDetails,
        CancellationToken cancellationToken = default);
}