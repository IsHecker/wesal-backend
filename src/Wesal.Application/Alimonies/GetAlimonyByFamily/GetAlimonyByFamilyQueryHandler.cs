using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Alimonies.GetAlimonyByFamily;
using Wesal.Application.Messaging;
using Wesal.Contracts.Alimonies;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class GetAlimonyByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IAlimonyRepository alimonyRepository)
    : IQueryHandler<GetAlimonyByFamilyQuery, AlimonyResponse>
{
    public async Task<Result<AlimonyResponse>> Handle(
        GetAlimonyByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        var alimonies = await alimonyRepository.GetByFamilyIdAsync(request.FamilyId, cancellationToken);
        if (alimonies is null)
            return AlimonyErrors.NotFound;

        return new AlimonyResponse(
            alimonies.Id,
            alimonies.CourtId,
            alimonies.CourtCaseId,
            alimonies.PayerId,
            alimonies.RecipientId,
            alimonies.FamilyId,
            alimonies.Amount,
            alimonies.Frequency.ToString(),
            alimonies.StartDate,
            alimonies.EndDate);
    }
}