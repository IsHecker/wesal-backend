using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.ObligationAlerts;

public static class ObligationAlertErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("ObligationAlert.NotFound", $"Obligation alert with ID '{id}' was not found");

    public static Error Unauthorized() =>
        Error.Unauthorized("ObligationAlert.Unauthorized", "You are not authorized to resolve this alert");

    public static Error AlreadyResolved() =>
        Error.Conflict("ObligationAlert.AlreadyResolved", "This alert has already been resolved");

    public static Error AlreadyUnderReview() =>
        Error.Conflict("ObligationAlert.AlreadyUnderReview", "This alert is already under review");

    public static Error CannotModifyResolved() =>
        Error.Conflict("ObligationAlert.CannotModifyResolved", "Cannot modify a resolved alert");
}