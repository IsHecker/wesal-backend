using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Alimonies.CreateAlimony;

internal sealed class CreateAlimonyCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    IFamilyRepository familyRepository,
    IParentRepository parentRepository,
    IAlimonyRepository alimonyRepository)
    : ICommandHandler<CreateAlimonyCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateAlimonyCommand request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        var validationResult = await ValidateAlimony(request, courtCase, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var alimony = Alimony.Create(
            courtCase,
            request.PayerId,
            request.RecipientId,
            request.Amount,
            request.Frequency.ToEnum<AlimonyFrequency>(),
            request.StartDate,
            request.EndDate);

        await alimonyRepository.AddAsync(alimony, cancellationToken);

        return alimony.Id;
    }

    private async Task<Result> ValidateAlimony(
        CreateAlimonyCommand request,
        CourtCase courtCase,
        CancellationToken cancellationToken)
    {
        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var family = await familyRepository.GetByIdAsync(courtCase.FamilyId, cancellationToken)
            ?? throw new InvalidOperationException();

        var alimonyExists = await alimonyRepository.ExistsByCourtCaseIdAsync(request.CourtCaseId, cancellationToken);
        if (alimonyExists)
            return AlimonyErrors.AlreadyExists(request.CourtCaseId);

        var payer = await parentRepository.GetByIdAsync(request.PayerId, cancellationToken);
        if (payer is null)
            return ParentErrors.NotFound(request.PayerId);

        var recipient = await parentRepository.GetByIdAsync(request.RecipientId, cancellationToken);
        if (recipient is null)
            return ParentErrors.NotFound(request.RecipientId);

        if (family.FatherId != request.PayerId && family.MotherId != request.PayerId)
            return AlimonyErrors.PayerNotInFamily;

        if (family.FatherId != request.RecipientId && family.MotherId != request.RecipientId)
            return AlimonyErrors.RecipientNotInFamily;

        if (request.PayerId == request.RecipientId)
            return AlimonyErrors.PayerAndRecipientSame;

        return Result.Success;
    }
}