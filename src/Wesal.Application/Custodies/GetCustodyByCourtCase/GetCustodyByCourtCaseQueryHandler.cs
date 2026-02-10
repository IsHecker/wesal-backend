using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Custodies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Results;

namespace Wesal.Application.Custodies.GetCustodyByCourtCase;

internal sealed class GetCustodyByCourtCaseQueryHandler(
    ICourtCaseRepository courtCaseRepository,
    IWesalDbContext dbContext)
    : IQueryHandler<GetCustodyByCourtCaseQuery, CustodyResponse>
{
    public async Task<Result<CustodyResponse>> Handle(
        GetCustodyByCourtCaseQuery request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        // if (courtCase.CourtId != request.CourtId)
        //     return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var custody = await dbContext.Custodies
            .FirstOrDefaultAsync(custody => custody.CourtCaseId == request.CourtCaseId, cancellationToken);

        if (custody is null)
            return CustodyErrors.NotFoundForCourtCase(request.CourtCaseId);

        return new CustodyResponse(
            custody.Id,
            custody.CourtCaseId,
            custody.FamilyId,
            custody.CustodialParentId,
            custody.StartAt,
            custody.EndAt);
    }
}