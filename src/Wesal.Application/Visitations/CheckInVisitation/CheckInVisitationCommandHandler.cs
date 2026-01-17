using Microsoft.Extensions.Options;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CheckInVisitation;

internal sealed class CheckInVisitationCommandHandler(
    IRepository<Visitation> visitationRepository,
    IRepository<VisitCenterStaff> centerStaffRepository,
    IOptions<VisitationOptions> options)
    : ICommandHandler<CheckInVisitationCommand>
{
    public async Task<Result> Handle(
        CheckInVisitationCommand request,
        CancellationToken cancellationToken)
    {
        var visitation = await visitationRepository.GetByIdAsync(request.VisitationId, cancellationToken);
        if (visitation is null)
            return VisitationErrors.NotFound(request.VisitationId);

        var staff = await centerStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var checkInResult = visitation.CheckIn(staff.SomeLocationId, options.Value.GracePeriodMinutes);
        if (checkInResult.IsFailure)
            return checkInResult.Error;

        visitationRepository.Update(visitation);

        return Result.Success;
    }
}