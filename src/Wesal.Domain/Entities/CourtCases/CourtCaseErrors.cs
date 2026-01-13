using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CourtCases;

public static class CourtCaseErrors
{
    public static Error CaseNumberAlreadyExists(string caseNumber) =>
        Error.Conflict("CourtCase.CaseNumberAlreadyExists", $"Case number '{caseNumber}' already exists");

    public static Error NotFoundForFamily(Guid familyId) =>
        Error.NotFound("CourtCase.NotFoundForFamily", $"No court case found for family with ID '{familyId}'");
}