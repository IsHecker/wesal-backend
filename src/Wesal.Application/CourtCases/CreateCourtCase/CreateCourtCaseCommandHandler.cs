using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.CreateCourtCase;

internal sealed class CreateCourtCaseHandler(
    ICourtCaseRepository courtCaseRepository,
    IFamilyRepository familyRepository) : ICommandHandler<CreateCourtCaseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCourtCaseCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (await courtCaseRepository.ExistsByCaseNumberAsync(request.CaseNumber, cancellationToken))
            return CourtCaseErrors.CaseNumberAlreadyExists(request.CaseNumber);

        // TODO: Upload court order document (optional)

        var courtCase = CourtCase.Create(
            request.CourtId,
            request.FamilyId,
            request.CaseNumber,
            request.FiledAt,
            request.Status.ToEnum<CourtCaseStatus>(),
            request.DecisionSummary);

        await courtCaseRepository.AddAsync(courtCase, cancellationToken);

        return courtCase.Id;
    }
}