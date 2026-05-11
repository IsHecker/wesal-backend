using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

internal sealed class UpdateObligationAlertStatusCommandHandler(
    IObligationAlertRepository alertRepository,
    ICourtStaffRepository courtStaffRepository)
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

        if (alert.AssignedStaffId != request.StaffId)
            return Error.Forbidden("ObligationAlert.Ownership", "You are not assigned to this alert.");

        var updateResult = alert.UpdateStatus(request.Status.ToEnum<AlertStatus>(), request.ResolutionNotes);
        if (updateResult.IsFailure)
            return updateResult;

        if (alert.Status == AlertStatus.Resolved)
        {
            var staff = await courtStaffRepository.GetByIdWithWorkloadAsync(alert.AssignedStaffId, cancellationToken);
            staff!.DecrementLoad(AssignmentType.ObligationAlert);
            // courtStaffRepository.Update(staff);
        }

        alertRepository.Update(alert);
        return Result.Success;
    }
}