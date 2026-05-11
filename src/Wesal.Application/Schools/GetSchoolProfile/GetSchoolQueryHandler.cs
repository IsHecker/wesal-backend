using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.GetSchool;

internal sealed class GetSchoolQueryHandler(ISchoolRepository schoolRepository)
    : IQueryHandler<GetSchoolQuery, SchoolResponse>
{
    public async Task<Result<SchoolResponse>> Handle(
        GetSchoolQuery request,
        CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFound(request.SchoolId);

        return school.ToResponse();
    }
}