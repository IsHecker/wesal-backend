using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.CourtCases;

internal sealed class CourtCaseRepository(WesalDbContext context)
    : Repository<CourtCase>(context), ICourtCaseRepository
{
    public Task<bool> ExistsByCaseNumberAsync(string caseNumber, CancellationToken cancellationToken = default)
    {
        return context.CourtCases
            .AnyAsync(courtCase => courtCase.CaseNumber == caseNumber, cancellationToken);
    }

    public Task<bool> HasOpenCaseByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return context.CourtCases
            .AnyAsync(courtCase => courtCase.FamilyId == familyId
                && courtCase.Status == CourtCaseStatus.Open, cancellationToken);

    }
}