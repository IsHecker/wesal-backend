using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

internal sealed class UpdateObligationAlertStatusCommandHandler(
    IObligationAlertRepository alertRepository,
    ICourtStaffRepository staffRepository) : ICommandHandler<UpdateObligationAlertStatusCommand>
{
    public async Task<Result> Handle(
        UpdateObligationAlertStatusCommand request,
        CancellationToken cancellationToken)
    {
        var alert = await alertRepository.GetByIdAsync(request.AlertId, cancellationToken);

        if (alert is null)
            return ObligationAlertErrors.NotFound(request.AlertId);

        var staff = await staffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.UserId);

        if (alert.CourtId != staff.CourtId)
            return ObligationAlertErrors.Unauthorized();

        alertRepository.Update(alert);

        return alert.UpdateStatus(request.Status.ToEnum<AlertStatus>(), request.ResolutionNotes);
    }
}