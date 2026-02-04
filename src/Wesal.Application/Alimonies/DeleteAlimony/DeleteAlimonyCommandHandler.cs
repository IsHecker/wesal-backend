using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Alimonies.DeleteAlimony;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class DeleteAlimonyCommandHandler(
    IAlimonyRepository alimonyRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<DeleteAlimonyCommand>
{
    public async Task<Result> Handle(
        DeleteAlimonyCommand request,
        CancellationToken cancellationToken)
    {
        var alimony = await alimonyRepository.GetByIdAsync(request.AlimonyId, cancellationToken);
        if (alimony is null)
            return AlimonyErrors.NotFound;

        var courtCase = await courtCaseRepository.GetByIdAsync(alimony.CourtCaseId, cancellationToken);

        if (courtCase!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        if (courtCase.Status == CourtCaseStatus.Closed)
            return AlimonyErrors.CannotModifyClosedCase;

        alimonyRepository.Delete(alimony);

        return Result.Success;
    }
}