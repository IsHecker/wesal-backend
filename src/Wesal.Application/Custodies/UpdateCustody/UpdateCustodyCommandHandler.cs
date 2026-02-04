using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Custodies.UpdateCustody;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class UpdateCustodyCommandHandler(
    ICustodyRepository custodyRepository,
    IFamilyRepository familyRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<UpdateCustodyCommand>
{
    public async Task<Result> Handle(
        UpdateCustodyCommand request,
        CancellationToken cancellationToken)
    {
        var custody = await custodyRepository.GetByIdAsync(request.CustodyId, cancellationToken);
        if (custody is null)
            return CustodyErrors.NotFound(request.CustodyId);

        var courtCase = await courtCaseRepository.GetByIdAsync(custody.CourtCaseId, cancellationToken);

        if (courtCase!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        if (courtCase!.Status == CourtCaseStatus.Closed)
            return CustodyErrors.CannotModifyClosedCase;

        var isInFamily = await familyRepository.IsParentInFamilyAsync(
            request.NewCustodianId,
            custody.FamilyId,
            cancellationToken);

        if (!isInFamily)
            return CustodyErrors.NewCustodianNotInFamily;

        custody.Update(request.NewCustodianId, request.StartAt, request.EndAt);

        return Result.Success;
    }
}