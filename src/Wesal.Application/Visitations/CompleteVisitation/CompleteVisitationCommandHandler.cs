using Microsoft.Extensions.Options;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CompleteVisitation;

internal sealed class CompleteVisitationCommandHandler(
    IRepository<Visitation> visitationRepository,
    IRepository<VisitCenterStaff> centerStaffRepository,
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

        var staff = await centerStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return VisitCenterStaffErrors.NotFound(request.StaffId);

        var CompleteResult = visitation.Complete(staff, options.Value.GracePeriodMinutes);
        if (CompleteResult.IsFailure)
            return CompleteResult.Error;

        visitationRepository.Update(visitation);

        return Result.Success;
    }
}