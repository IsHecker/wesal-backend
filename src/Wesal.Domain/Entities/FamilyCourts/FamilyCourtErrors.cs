using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.FamilyCourts;

public static class FamilyCourtErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("FamilyCourt.NotFound", $"Family Court with ID '{id}' was not found");

    public static Error NotBelongToCourt(string entityName) =>
        Error.Forbidden("FamilyCourt.NotBelongToCourt", $"This {entityName} doesn't belong your court");
}