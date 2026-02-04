using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Messaging;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.FamilyCourts.CreateFamilyCourt;

internal sealed class CreateFamilyCourtCommandHandler(
    IFamilyCourtRepository familyCourtRepository,
    IUserService userService)
    : ICommandHandler<CreateFamilyCourtCommand, UserCredentialResponse>
{
    public async Task<Result<UserCredentialResponse>> Handle(
        CreateFamilyCourtCommand request,
        CancellationToken cancellationToken)
    {
        var isExisting = await userService.ExistsByEmailAsync<FamilyCourt>(
            request.Email,
            cancellationToken);

        if (isExisting)
            return UserErrors.EmailAlreadyExists(request.Email);

        var userResult = await userService.CreateAsync(
            UserRole.FamilyCourt,
            cancellationToken);

        var familyCourt = FamilyCourt.Create(
            userResult.User.Id,
            request.Email,
            request.Name,
            request.Governorate,
            request.Address,
            request.ContactInfo);

        await familyCourtRepository.AddAsync(familyCourt, cancellationToken);

        return new UserCredentialResponse(familyCourt.Id, familyCourt.Name, userResult.TemporaryPassword);
    }
}