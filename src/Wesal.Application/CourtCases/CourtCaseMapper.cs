using Wesal.Contracts.CourtCases;
using Wesal.Domain.Entities.CourtCases;

namespace Wesal.Application.CourtCases;

public static class CourtCaseMapper
{
    public static CourtCaseResponse ToResponse(this CourtCase courtCase) =>
        new(
            courtCase.Id,
            courtCase.CourtId,
            courtCase.FamilyId,
            courtCase.CaseNumber,
            courtCase.FiledAt,
            courtCase.Status.ToString(),
            courtCase.DecisionSummary);
}