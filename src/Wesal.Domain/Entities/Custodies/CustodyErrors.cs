using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Custodies;

public static class CustodyErrors
{
    public static Error NotFoundForFamily(Guid familyId) =>
        Error.NotFound(
            "Custody.NotFoundForFamily",
            $"No custody decision found for family with ID '{familyId}'");
}