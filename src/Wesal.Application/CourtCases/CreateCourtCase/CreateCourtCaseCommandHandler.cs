using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.CreateCourtCase;

internal sealed class CreateCourtCaseHandler(
    ICourtCaseRepository courtCaseRepository,
    ICourtStaffRepository staffRepository,
    IFamilyRepository familyRepository)
    : ICommandHandler<CreateCourtCaseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCourtCaseCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.UserId);

        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (await courtCaseRepository.ExistsByCaseNumberAsync(request.CaseNumber, cancellationToken))
            return CourtCaseErrors.CaseNumberAlreadyExists(request.CaseNumber);

        // TODO: Upload court order document (optional)

        var courtCase = CourtCase.Create(
            staff.CourtId,
            request.FamilyId,
            request.CaseNumber,
            request.Status.ToEnum<CourtCaseStatus>(),
            request.DecisionSummary);

        await courtCaseRepository.AddAsync(courtCase, cancellationToken);

        return courtCase.Id;
    }
}