using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Children;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Children.ListChildrenByFamily;

internal sealed class ListChildrenByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListChildrenByFamilyQuery, IEnumerable<ChildResponse>>
{
    public async Task<Result<IEnumerable<ChildResponse>>> Handle(
        ListChildrenByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        var query = context.Children
            .Where(c => c.FamilyId == request.FamilyId);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .OrderBy(c => c.FullName)
            .Select(child => new ChildResponse(
                child.Id,
                child.FullName,
                child.SchoolId,
                child.Gender,
                child.BirthDate,
                AgeCalculator.CalculateAge(child.BirthDate)))
            .ToListAsync(cancellationToken);
    }
}