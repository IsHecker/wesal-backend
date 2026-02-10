using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CheckInVisitation;

internal sealed class CheckInVisitationCommandHandler(
    IVisitationRepository visitationRepository,
    IVisitCenterStaffRepository centerStaffRepository,
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

        var centerStaff = await centerStaffRepository.GetByIdAsync(request.CenterStaffId, cancellationToken);
        if (centerStaff is null)
            return UserErrors.NotFound(request.CenterStaffId);

        var checkInResult = visitation.CheckIn(centerStaff.LocationId, request.NationalId, options.Value.CheckInGracePeriodMinutes);
        if (checkInResult.IsFailure)
            return checkInResult.Error;

        visitationRepository.Update(visitation);

        return Result.Success;
    }
}