using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Custodies;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Custodies.GetCustodyByFamily;

internal sealed class GetCustodyByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    ICustodyRepository custodyRepository)
    : IQueryHandler<GetCustodyByFamilyQuery, CustodyResponse>
{
    public async Task<Result<CustodyResponse>> Handle(
        GetCustodyByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var isFamilyExist = await familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (!isFamilyExist)
            return FamilyErrors.NotFound(request.FamilyId);

        var custody = await custodyRepository.GetByFamilyIdAsync(request.FamilyId, cancellationToken);

        if (custody is null)
            return CustodyErrors.NotFoundForFamily(request.FamilyId);

        return new CustodyResponse(
            custody.Id,
            custody.CourtCaseId,
            custody.FamilyId,
            custody.CustodianId,
            custody.StartAt,
            custody.EndAt);
    }
}

