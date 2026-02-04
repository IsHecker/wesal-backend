using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

internal sealed class UpdateObligationAlertStatusCommandHandler(
    IObligationAlertRepository alertRepository)
    : ICommandHandler<UpdateObligationAlertStatusCommand>
{
    public async Task<Result> Handle(
        UpdateObligationAlertStatusCommand request,
        CancellationToken cancellationToken)
    {
        var alert = await alertRepository.GetByIdAsync(request.AlertId, cancellationToken);
        if (alert is null)
            return ObligationAlertErrors.NotFound(request.AlertId);

        if (alert.CourtId != request.CourtId)
            return ObligationAlertErrors.AlertMismatch;

        alertRepository.Update(alert);

        return alert.UpdateStatus(request.Status.ToEnum<AlertStatus>(), request.ResolutionNotes);
    }
}