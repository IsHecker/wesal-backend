using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Custodies.CreateCustody;

internal sealed class CreateCustodyCommandHandler(
    ICourtStaffRepository courtStaffRepository,
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
        var staff = await courtStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        var validationResult = await ValidateCustody(request, courtCase.FamilyId, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var custody = Custody.Create(
            request.CourtCaseId,
            courtCase.FamilyId,
            request.CustodianId,
            request.StartAt,
            request.EndAt);

        await custodyRepository.AddAsync(custody, cancellationToken);

        return custody.Id;
    }

    private async Task<Result> ValidateCustody(
        CreateCustodyCommand request,
        Guid familyId,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(familyId, cancellationToken)
            ?? throw new InvalidOperationException();

        var custodyExists = await custodyRepository.ExistsByCourtCaseIdAsync(request.CourtCaseId, cancellationToken);
        if (custodyExists)
            return CustodyErrors.AlreadyExists(request.CourtCaseId);

        var custodian = await parentRepository.GetByIdAsync(request.CustodianId, cancellationToken);
        if (custodian is null)
            return ParentErrors.NotFound(request.CustodianId);

        if (family.FatherId != request.CustodianId && family.MotherId != request.CustodianId)
            return CustodyErrors.CustodianNotInFamily;

        return Result.Success;
    }
}
