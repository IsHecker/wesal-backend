using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.GetFamilyByParent;

public sealed class GetFamilyByParentQueryHandler(
    IFamilyRepository familyRepository)
    : IQueryHandler<GetFamilyByParentQuery, FamilyResponse>
{
    public async Task<Result<FamilyResponse>> Handle(
        GetFamilyByParentQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: support multiple families for Father.

        var family = await familyRepository.GetByParentIdAsync(request.ParentId, cancellationToken);

        if (family is null)
            return FamilyErrors.NotFoundByParent(request.ParentId);

        return family.ToResponse();
    }
}