using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CompleteVisitation;

internal sealed class CompleteVisitationCommandHandler(
    IRepository<Visitation> visitationRepository,
    IVisitCenterStaffRepository centerStaffRepository,
    IComplianceMetricsRepository metricsRepository,
    IOptions<VisitationOptions> options)
    : ICommandHandler<CompleteVisitationCommand>
{
    public async Task<Result> Handle(
        CompleteVisitationCommand request,
        CancellationToken cancellationToken)
    {
        var visitation = await visitationRepository.GetByIdAsync(request.VisitationId, cancellationToken);
        if (visitation is null)
            return VisitationErrors.NotFound(request.VisitationId);

        var staff = await centerStaffRepository.GetByIdAsync(request.CenterStaffId, cancellationToken);
        if (staff is null)
            return VisitCenterStaffErrors.NotFound(request.CenterStaffId);

        var CompleteResult = visitation.Complete(staff, options.Value.CheckInGracePeriodMinutes);
        if (CompleteResult.IsFailure)
            return CompleteResult.Error;

        visitationRepository.Update(visitation);
        await RecordVisitationCompletedAsync(visitation, cancellationToken);

        return Result.Success;
    }

    private async Task RecordVisitationCompletedAsync(Visitation visitation, CancellationToken cancellationToken)
    {
        var metrics = await metricsRepository.GetAsync(
            visitation.FamilyId,
            visitation.ParentId,
            DateTime.UtcNow.ToDateOnly(),
            cancellationToken) ?? throw new InvalidOperationException();

        metrics.RecordVisitationCompleted();
        metricsRepository.Update(metrics);
    }
}