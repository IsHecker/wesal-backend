using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.VisitationLocations;

public static class VisitationLocationErrors
{
    public static Error NotFound(Guid locationId) =>
        Error.NotFound(
            "VisitLocation.NotFound",
            $"Visit location with ID '{locationId}' was not found");

    public static Error AlreadyExists(string name, string governorate) =>
        Error.Conflict(
            "VisitLocation.AlreadyExists",
            $"Visit location '{name}' in '{governorate}' already exists");
    public static Error Unauthorized() =>
        Error.Forbidden(
            "VisitLocation.Unauthorized",
            "You are not authorized to update this visit location");
}