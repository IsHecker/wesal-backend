using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.RegisterSchool;

internal sealed class RegisterSchoolCommandHandler(
    IRepository<User> userRepository,
    IRepository<School> schoolRepository)
    : ICommandHandler<RegisterSchoolCommand, RegisterSchoolResponse>
{
    public async Task<Result<RegisterSchoolResponse>> Handle(
        RegisterSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var schoolUser = User.Create("School");
        var school = School.Create(
            schoolUser.Id,
            request.Name,
            request.Address,
            request.Governorate,
            request.ContactNumber);

        await userRepository.AddAsync(schoolUser, cancellationToken);
        await schoolRepository.AddAsync(school, cancellationToken);

        return default!;
    }
}