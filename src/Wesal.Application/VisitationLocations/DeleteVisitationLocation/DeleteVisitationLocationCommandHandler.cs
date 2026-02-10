using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Application.VisitationLocations.DeleteVisitationLocation;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

internal sealed class DeleteVisitationLocationCommandHandler(
    IVisitationLocationRepository visitationLocationRepository,
    IVisitationRepository visitationRepository)
    : ICommandHandler<DeleteVisitationLocationCommand>
{
    public async Task<Result> Handle(
        DeleteVisitationLocationCommand request,
        CancellationToken cancellationToken)
    {
        var location = await visitationLocationRepository.GetByIdAsync(request.LocationId, cancellationToken);
        if (location is null)
            return VisitationLocationErrors.NotFound(request.LocationId);

        var isInUse = await visitationRepository.ExistsByLocationIdAsync(request.LocationId, cancellationToken);
        if (isInUse)
            return VisitationLocationErrors.InUseByVisitations;

        if (location!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(VisitationLocation));

        visitationLocationRepository.Delete(location);

        return Result.Success;
    }
}