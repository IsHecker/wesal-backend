using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.GetFamily;

internal sealed class GetFamilyQueryHandler(
    IWesalDbContext context)
    : IQueryHandler<GetFamilyQuery, FamilyResponse>
{
    public async Task<Result<FamilyResponse>> Handle(
        GetFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var family = await context.Families
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .FirstOrDefaultAsync(family => family.Id == request.FamilyId, cancellationToken);

        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        return family.ToResponse();
    }
}