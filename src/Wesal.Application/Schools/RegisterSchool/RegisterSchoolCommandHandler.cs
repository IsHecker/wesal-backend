using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Messaging;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.RegisterSchool;

internal sealed class RegisterSchoolCommandHandler(
    IFamilyCourtRepository courtRepository,
    ISchoolRepository schoolRepository,
    IUserService userService)
    : ICommandHandler<RegisterSchoolCommand, UserCredentialResponse>
{
    public async Task<Result<UserCredentialResponse>> Handle(
        RegisterSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var court = await courtRepository.GetByIdAsync(request.CourtId, cancellationToken);
        if (court is null)
            return FamilyCourtErrors.NotFound(request.CourtId);

        var validationResult = await ValidateSchool(request, court.Governorate, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var userResult = await userService.CreateAsync(UserRole.School, cancellationToken);

        var school = School.Create(
            userResult.User.Id,
            GenerateUsername(court.Governorate),
            request.Name,
            request.Address,
            court.Governorate,
            request.ContactNumber);

        await schoolRepository.AddAsync(school, cancellationToken);

        return new UserCredentialResponse(
            school.Id,
            school.Username,
            userResult.TemporaryPassword);
    }

    private async Task<Result> ValidateSchool(
        RegisterSchoolCommand request,
        string governorate,
        CancellationToken cancellationToken)
    {
        var nameExists = await schoolRepository.ExistsByNameAndGovernorateAsync(
            request.Name,
            governorate,
            cancellationToken);

        if (nameExists)
            return SchoolErrors.SchoolAlreadyExists(request.Name, governorate);

        return Result.Success;
    }

    private static string GenerateUsername(string governorate)
    {
        var randomNumber = Random.Shared.Next(1000, 10_000);
        return $"sch-{governorate}-{randomNumber}";
    }
}