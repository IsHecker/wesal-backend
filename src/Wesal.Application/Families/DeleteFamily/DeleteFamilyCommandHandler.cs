using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.DeleteFamily;

internal sealed class DeleteFamilyCommandHandler(
    IFamilyRepository familyRepository)
    : ICommandHandler<DeleteFamilyCommand>
{
    public async Task<Result> Handle(
        DeleteFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        // TODO: Block if an active (non-closed) court case exists ---
        // var hasActiveCase = await courtCaseRepository
        //     .HasActiveByCaseByFamilyIdAsync(request.FamilyId, cancellationToken);

        // TODO: Block if any completed visitation exists (legal evidence) ---
        // var hasCompletedVisitations = await visitationRepository
        //     .HasCompletedByFamilyIdAsync(request.FamilyId, cancellationToken);

        // TODO: Block if any paid alimony exists (legal/financial evidence) ---
        // var hasPaidAlimony = await alimonyRepository
        //     .HasPaidByFamilyIdAsync(request.FamilyId, cancellationToken);

        // if (hasActiveCase || hasCompletedVisitations || hasPaidAlimony)
        //     return FamilyErrors.HasActiveCaseOrEvidence;

        if (family!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        familyRepository.Delete(family);

        return Result.Success;
    }
}