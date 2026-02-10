using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.Alimonies.CreateAlimony;

internal sealed class CreateAlimonyCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    ICustodyRepository custodyRepository,
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

        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var custody = await custodyRepository.GetByFamilyIdAsync(courtCase.FamilyId, cancellationToken);

        var alimonyExists = await alimonyRepository.ExistsByCourtCaseIdAsync(request.CourtCaseId, cancellationToken);
        if (alimonyExists)
            return AlimonyErrors.AlreadyExists(request.CourtCaseId);

        var alimony = Alimony.Create(
            courtCase,
            custody!.NonCustodialParentId,
            custody.CustodialParentId,
            request.Amount,
            request.Frequency.ToEnum<AlimonyFrequency>(),
            request.StartDate,
            request.EndDate);

        await alimonyRepository.AddAsync(alimony, cancellationToken);

        return alimony.Id;
    }
}