using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Messaging;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitCenterStaffs.CreateVisitCenterStaff;

internal sealed class CreateVisitCenterStaffCommandHandler(
    IVisitCenterStaffRepository visitCenterStaffRepository,
    IVisitationLocationRepository visitLocationRepository,
    IUserService userService)
    : ICommandHandler<CreateVisitCenterStaffCommand, UserCredentialResponse>
{
    public async Task<Result<UserCredentialResponse>> Handle(
        CreateVisitCenterStaffCommand request,
        CancellationToken cancellationToken)
    {
        var location = await visitLocationRepository.GetByIdAsync(
            request.LocationId,
            cancellationToken);

        if (location is null)
            return VisitationLocationErrors.NotFound(request.LocationId);

        if (location.CourtId != request.CourtId)
            return UserErrors.CrossCourtAccessDenied;

        var isExisting = await userService.ExistsByEmailAsync<VisitCenterStaff>(
            request.Email,
            cancellationToken);

        if (isExisting)
            return UserErrors.EmailAlreadyExists(request.Email);

        var userResult = await userService.CreateAsync(UserRole.VisitCenterStaff, cancellationToken);

        var centerStaff = VisitCenterStaff.Create(
            userResult.User.Id,
            request.CourtId,
            request.LocationId,
            request.Email,
            request.FullName,
            request.Phone);

        await visitCenterStaffRepository.AddAsync(centerStaff, cancellationToken);

        return new UserCredentialResponse(centerStaff.Id, centerStaff.FullName, userResult.TemporaryPassword);
    }
}