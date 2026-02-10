using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationLocations.UpdateVisitationLocation;

internal sealed class UpdateVisitationLocationCommandHandler(
    IVisitationLocationRepository visitLocationRepository)
    : ICommandHandler<UpdateVisitationLocationCommand>
{
    public async Task<Result> Handle(
        UpdateVisitationLocationCommand request,
        CancellationToken cancellationToken)
    {
        var location = await visitLocationRepository.GetByIdAsync(request.LocationId, cancellationToken);
        if (location is null)
            return VisitationLocationErrors.NotFound(request.LocationId);

        if (location.CourtId != request.CourtId)
            return VisitationLocationErrors.Unauthorized;

        location.Update(
            request.Name,
            request.Address,
            request.Governorate,
            request.ContactNumber,
            request.MaxConcurrentVisits,
            request.OpeningTime,
            request.ClosingTime);

        visitLocationRepository.Update(location);

        return Result.Success;
    }
}