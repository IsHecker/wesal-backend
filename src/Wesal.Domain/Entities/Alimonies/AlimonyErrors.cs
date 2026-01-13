using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Alimonies;

public static class AlimonyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("AlimonyObligation.NotFound", $"Alimony obligation with ID '{id}' was not found");
}