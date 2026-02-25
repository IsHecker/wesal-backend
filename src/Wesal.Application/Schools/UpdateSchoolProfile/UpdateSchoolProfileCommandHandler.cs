using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.UpdateSchoolProfile;

internal sealed class UpdateSchoolProfileCommandHandler(ISchoolRepository schoolRepository)
    : ICommandHandler<UpdateSchoolProfileCommand>
{
    public async Task<Result> Handle(UpdateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFound(request.SchoolId);

        school.UpdateProfile(request.Email, request.ContactNumber);
        schoolRepository.Update(school);
        return Result.Success;
    }
}