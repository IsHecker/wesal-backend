using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CheckInVisitation;

internal sealed class CheckInVisitationCommandHandler(
    IRepository<Visitation> visitationRepository,
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

        var staff = await centerStaffRepository.GetByIdAsync(request.CenterStaffId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.CenterStaffId);

        var checkInResult = visitation.CheckIn(staff.LocationId, options.Value.CheckInGracePeriodMinutes);
        if (checkInResult.IsFailure)
            return checkInResult.Error;

        visitationRepository.Update(visitation);

        return Result.Success;
    }
}