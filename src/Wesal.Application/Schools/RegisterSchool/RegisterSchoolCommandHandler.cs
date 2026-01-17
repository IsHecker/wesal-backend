using System.Security.Cryptography;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.RegisterSchool;

internal sealed class RegisterSchoolCommandHandler(
    IRepository<User> userRepository,
    ISchoolRepository schoolRepository)
    : ICommandHandler<RegisterSchoolCommand, RegisterSchoolResponse>
{
    public async Task<Result<RegisterSchoolResponse>> Handle(
        RegisterSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateSchool(request, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var credentials = GenerateCredentials(request.Governorate);

        var user = User.Create(
            UserRole.School,
            credentials.Username,
            credentials.Password);

        var school = School.Create(
            user.Id,
            request.Name,
            request.Address,
            request.Governorate,
            request.ContactNumber);

        await userRepository.AddAsync(user, cancellationToken);
        await schoolRepository.AddAsync(school, cancellationToken);

        return new RegisterSchoolResponse(
            school.Id,
            credentials.Username,
            credentials.Password);
    }

    private async Task<Result> ValidateSchool(RegisterSchoolCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await schoolRepository.ExistsByNameAndGovernorateAsync(
            request.Name,
            request.Governorate,
            cancellationToken);

        if (nameExists)
            return SchoolErrors.SchoolAlreadyExists(request.Name, request.Governorate);

        return Result.Success;
    }

    private static SchoolCredentials GenerateCredentials(string governorate)
    {
        var randomNumber = Random.Shared.Next(1000, 10_000);
        var userName = $"sch-{governorate}-{randomNumber}";

        return new SchoolCredentials(userName, GeneratePassword());
    }

    private static string GeneratePassword()
    {
        const int base64Length = 8;
        const int keyBytes = base64Length / 4 * 3;

        Span<byte> bytes = stackalloc byte[keyBytes];
        RandomNumberGenerator.Fill(bytes);

        Span<char> base64Chars = stackalloc char[base64Length];
        Convert.TryToBase64Chars(bytes, base64Chars, out _);

        base64Chars.Replace('+', RandomLowerCase());
        base64Chars.Replace('=', RandomLowerCase());
        base64Chars.Replace('/', RandomNumber());

        return new string(base64Chars);

        static char RandomLowerCase() => (char)RandomNumberGenerator.GetInt32(97, 123);
        static char RandomNumber() => (char)RandomNumberGenerator.GetInt32(48, 58);
    }

    private record struct SchoolCredentials(string Username, string Password);
}