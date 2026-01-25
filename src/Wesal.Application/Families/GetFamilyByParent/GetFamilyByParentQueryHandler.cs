using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.GetFamilyByParent;

internal sealed class GetFamilyByParentQueryHandler(
    IRepository<User> userRepository,
    IParentRepository parentRepository,
    IWesalDbContext context)
    : IQueryHandler<GetFamilyByParentQuery, FamilyResponse>
{
    public async Task<Result<FamilyResponse>> Handle(
        GetFamilyByParentQuery request,
        CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!isUserExist)
            return UserErrors.NotFound(request.UserId);

        // TODO: support multiple families for Father.

        var parent = await parentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var family = await context.Families
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .FirstOrDefaultAsync(family => family.FatherId == parent!.Id
                || family.MotherId == parent!.Id, cancellationToken);

        if (family is null)
            return FamilyErrors.NotFoundByParent(request.UserId);

        return family.ToResponse();
    }
}