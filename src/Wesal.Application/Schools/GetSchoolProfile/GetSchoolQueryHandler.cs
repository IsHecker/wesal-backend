using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.GetSchoolProfile;

internal sealed class GetSchoolQueryHandler(ISchoolRepository schoolRepository)
    : IQueryHandler<GetSchoolProfileQuery, SchoolResponse>
{
    public async Task<Result<SchoolResponse>> Handle(
        GetSchoolProfileQuery request,
        CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFound(request.SchoolId);

        return school.ToResponse();
    }
}