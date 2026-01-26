using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtCases;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.ListCourtCasesByFamily;

internal sealed class ListCourtCasesByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListCourtCasesByFamilyQuery, PagedResponse<CourtCaseResponse>>
{
    public async Task<Result<PagedResponse<CourtCaseResponse>>> Handle(
        ListCourtCasesByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var isFamilyExist = await familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (!isFamilyExist)
            return FamilyErrors.NotFound(request.FamilyId);

        var courtCases = context.CourtCases
            .Where(courtCase => courtCase.FamilyId == request.FamilyId);

        var totalCount = await courtCases.CountAsync(cancellationToken);

        return await courtCases
            .Paginate(request.Pagination)
            .Select(courtCase => courtCase.ToResponse())
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}