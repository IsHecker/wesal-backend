using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CourtCases;

public static class CourtCaseErrors
{
    public static Error CaseNumberAlreadyExists(string caseNumber) =>
        Error.Conflict("CourtCase.CaseNumberAlreadyExists", $"Case number '{caseNumber}' already exists");
}