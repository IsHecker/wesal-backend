using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CompleteVisitation;

internal sealed class CompleteVisitationCommandHandler(
    IRepository<Visitation> visitationRepository,
    IVisitCenterStaffRepository centerStaffRepository,
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

        var staff = await centerStaffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.UserId);

        var CompleteResult = visitation.Complete(staff, options.Value.CheckInGracePeriodMinutes);
        if (CompleteResult.IsFailure)
            return CompleteResult.Error;

        visitationRepository.Update(visitation);

        return Result.Success;
    }
}