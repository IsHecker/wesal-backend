using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.VisitationLocations;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationLocations.GetVisitationLocation;

internal sealed class GetVisitationLocationQueryHandler(
    IVisitationLocationRepository locationRepository)
    : IQueryHandler<GetVisitationLocationQuery, VisitationLocationResponse>
{
    public async Task<Result<VisitationLocationResponse>> Handle(
        GetVisitationLocationQuery request,
        CancellationToken cancellationToken)
    {
        var location = await locationRepository.GetByIdAsync(request.LocationId, cancellationToken);
        if (location is null)
            return VisitationLocationErrors.NotFound(request.LocationId);

        return new VisitationLocationResponse(
                location.Id,
                location.Name,
                location.Address,
                location.Governorate,
                location.ContactNumber,
                location.MaxConcurrentVisits,
                location.OpeningTime,
                location.ClosingTime);
    }
}