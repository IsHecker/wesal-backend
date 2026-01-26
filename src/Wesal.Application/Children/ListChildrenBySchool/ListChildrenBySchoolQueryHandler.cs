using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Children;
using Wesal.Contracts.Common;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Children.ListChildrenBySchool;

internal sealed class ListChildrenBySchoolQueryHandler(
    ISchoolRepository schoolRepository,
    IWesalDbContext context)
    : IQueryHandler<ListChildrenBySchoolQuery, PagedResponse<ChildResponse>>
{
    public async Task<Result<PagedResponse<ChildResponse>>> Handle(
        ListChildrenBySchoolQuery request,
        CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFound(request.SchoolId);

        var query = context.Children
            .Where(c => c.SchoolId == request.SchoolId)
            .OrderBy(c => c.FullName);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(child => new ChildResponse(
                child.Id,
                child.FullName,
                school.Id,
                child.Gender,
                child.BirthDate,
                AgeCalculator.CalculateAge(child.BirthDate)))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}