using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.ListFamiliesByParent;

internal sealed class ListFamiliesByParentQueryHandler(
    IParentRepository parentRepository,
    IWesalDbContext context)
    : IQueryHandler<ListFamiliesByParentQuery, IEnumerable<FamilyResponse>>
{
    public async Task<Result<IEnumerable<FamilyResponse>>> Handle(
        ListFamiliesByParentQuery request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);

        var families = await context.Families
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .Where(family => family.FatherId == parent!.Id || family.MotherId == parent!.Id)
            .Select(family => family.ToResponse())
            .ToListAsync(cancellationToken);

        if (families.Count == 0)
            return FamilyErrors.NotFoundByParent(request.ParentId);

        return families;
    }
}