using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Custodies.CreateCustody;

internal sealed class CreateCustodyCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    IFamilyRepository familyRepository,
    IParentRepository parentRepository,
    ICustodyRepository custodyRepository)
    : ICommandHandler<CreateCustodyCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCustodyCommand request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        var family = await familyRepository.GetByIdAsync(courtCase.FamilyId, cancellationToken)
            ?? throw new InvalidOperationException();

        var validationResult = await ValidateCustody(request, courtCase, family, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var nonCustodialParentId = request.CustodialParentId == family.MotherId
                ? family.FatherId : family.MotherId;

        var custody = Custody.Create(
            request.CourtCaseId,
            courtCase.FamilyId,
            request.CustodialParentId,
            nonCustodialParentId,
            request.StartAt,
            request.EndAt);

        await custodyRepository.AddAsync(custody, cancellationToken);

        return custody.Id;
    }

    private async Task<Result> ValidateCustody(
        CreateCustodyCommand request,
        CourtCase courtCase,
        Family family,
        CancellationToken cancellationToken)
    {
        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var custodyExists = await custodyRepository.ExistsByCourtCaseIdAsync(request.CourtCaseId, cancellationToken);
        if (custodyExists)
            return CustodyErrors.AlreadyExists(request.CourtCaseId);

        var custodian = await parentRepository.GetByIdAsync(request.CustodialParentId, cancellationToken);
        if (custodian is null)
            return ParentErrors.NotFound(request.CustodialParentId);

        if (family.FatherId != request.CustodialParentId && family.MotherId != request.CustodialParentId)
            return CustodyErrors.CustodianNotInFamily;

        return Result.Success;
    }
}