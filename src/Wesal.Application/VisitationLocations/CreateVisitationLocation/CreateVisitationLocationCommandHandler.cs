using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationLocations.CreateVisitationLocation;

internal sealed class CreateVisitationLocationCommandHandler(
    ICourtStaffRepository courtStaffRepository,
    IVisitationLocationRepository locationRepository)
    : ICommandHandler<CreateVisitationLocationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateVisitationLocationCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await courtStaffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.UserId);

        var nameExists = await locationRepository.ExistsByNameAndGovernorateAsync(
            request.Name,
            request.Governorate,
            cancellationToken);

        if (nameExists)
            return VisitationLocationErrors.AlreadyExists(request.Name, request.Governorate);

        var location = VisitationLocation.Create(
            staff.CourtId,
            request.Name,
            request.Address,
            request.Governorate,
            request.MaxConcurrentVisits,
            request.OpeningTime,
            request.ClosingTime,
            request.ContactNumber);

        await locationRepository.AddAsync(location, cancellationToken);

        return location.Id;
    }
}