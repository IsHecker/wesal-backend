using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.SetCompanionForVisitation;

internal sealed class SetCompanionForVisitationCommandHandler(
    IVisitationRepository visitationRepository,
    IVisitationScheduleRepository scheduleRepository)
    : ICommandHandler<SetCompanionForVisitationCommand>
{
    public async Task<Result> Handle(
        SetCompanionForVisitationCommand request,
        CancellationToken cancellationToken)
    {
        var visitation = await visitationRepository.GetByIdAsync(request.VisitationId, cancellationToken);
        if (visitation is null)
            return VisitationErrors.NotFound(request.VisitationId);

        var schedule = await scheduleRepository.GetByIdAsync(visitation.VisitationScheduleId, cancellationToken);
        if (schedule!.CustodialParentId != request.CustodialParentId)
            return Error.Forbidden();

        visitation.SetCompanion(request.CompanionNationalId);
        visitationRepository.Update(visitation);

        return Result.Success;
    }
}