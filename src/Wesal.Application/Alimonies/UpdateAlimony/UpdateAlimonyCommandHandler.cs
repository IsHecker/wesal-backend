using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Alimonies.UpdateAlimony;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class UpdateAlimonyCommandHandler(
    IAlimonyRepository alimonyRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<UpdateAlimonyCommand>
{
    public async Task<Result> Handle(
        UpdateAlimonyCommand request,
        CancellationToken cancellationToken)
    {
        var alimony = await alimonyRepository.GetByIdAsync(request.AlimonyId, cancellationToken);
        if (alimony is null)
            return AlimonyErrors.NotFound;

        var courtCase = await courtCaseRepository.GetByIdAsync(alimony.CourtCaseId, cancellationToken);

        if (courtCase!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        if (courtCase!.Status == CourtCaseStatus.Closed)
            return AlimonyErrors.CannotModifyClosedCase;

        alimony.Update(
            request.Amount,
            request.Frequency.ToEnum<AlimonyFrequency>(),
            request.StartDate,
            request.EndDate);

        alimonyRepository.Update(alimony);

        return Result.Success;
    }
}