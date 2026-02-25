using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationLocations.CreateVisitationLocation;

internal sealed class CreateVisitationLocationCommandHandler(
    IFamilyCourtRepository courtRepository,
    IVisitationLocationRepository locationRepository)
    : ICommandHandler<CreateVisitationLocationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateVisitationLocationCommand request,
        CancellationToken cancellationToken)
    {
        var court = await courtRepository.GetByIdAsync(request.CourtId, cancellationToken);
        if (court is null)
            return FamilyCourtErrors.NotFound(request.CourtId);

        var nameExists = await locationRepository.ExistsByNameAndGovernorateAsync(
            request.Name,
            court.Governorate,
            cancellationToken);

        if (nameExists)
            return VisitationLocationErrors.AlreadyExists(request.Name, court.Governorate);

        var location = VisitationLocation.Create(
            court.Id,
            request.Name,
            request.Address,
            court.Governorate,
            request.MaxConcurrentVisits,
            request.OpeningTime,
            request.ClosingTime,
            request.ContactNumber);

        await locationRepository.AddAsync(location, cancellationToken);

        return location.Id;
    }
}