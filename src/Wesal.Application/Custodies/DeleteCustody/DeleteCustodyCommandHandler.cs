using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Custodies.DeleteCustody;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class DeleteCustodyCommandHandler(
    ICustodyRepository custodyRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<DeleteCustodyCommand>
{
    public async Task<Result> Handle(
        DeleteCustodyCommand request,
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

        // TODO: Handle Deletion if it's still in use.

        // // --- Block if visitation schedules exist under this custody ---
        // var hasVisitationSchedules = await visitationScheduleRepository
        //     .ExistsByCustodyIdAsync(request.CustodyId, cancellationToken);

        // // --- Block if alimony obligations exist under this custody ---
        // var hasAlimony = await alimonyRepository
        //     .ExistsByCustodyIdAsync(request.CustodyId, cancellationToken);

        // if (hasVisitationSchedules || hasAlimony)
        //     return CustodyErrors.HasDependentRecords;

        custodyRepository.Delete(custody);

        return Result.Success;
    }
}