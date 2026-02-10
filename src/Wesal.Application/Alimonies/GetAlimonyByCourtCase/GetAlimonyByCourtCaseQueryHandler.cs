using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Alimonies;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.Alimonies.GetAlimonyByCourtCase;

internal sealed class GetAlimonyByCourtCaseQueryHandler(
    ICourtCaseRepository courtCaseRepository,
    IWesalDbContext dbContext)
    : IQueryHandler<GetAlimonyByCourtCaseQuery, AlimonyResponse>
{
    public async Task<Result<AlimonyResponse>> Handle(
        GetAlimonyByCourtCaseQuery request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var alimony = await dbContext.Alimonies
            .FirstOrDefaultAsync(alimony => alimony.CourtCaseId == request.CourtCaseId, cancellationToken);

        if (alimony is null)
            return AlimonyErrors.NotFound;

        return new AlimonyResponse(
            alimony.Id,
            alimony.CourtId,
            alimony.CourtCaseId,
            alimony.PayerId,
            alimony.RecipientId,
            alimony.FamilyId,
            alimony.Amount,
            alimony.Frequency.ToString(),
            alimony.StartDate,
            alimony.EndDate);
    }
}