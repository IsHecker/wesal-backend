using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.ObligationAlerts;

public static class ObligationAlertErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("ObligationAlert.NotFound", $"Obligation alert with ID '{id}' was not found");

    public static Error AlertMismatch =>
        Error.Unauthorized("ObligationAlert.AlertMismatch", "You are not authorized to resolve this alert");

    public static Error AlreadyResolved =>
        Error.Conflict("ObligationAlert.AlreadyResolved", "This alert has already been resolved");

    public static Error CannotUpdateStatus(AlertStatus status) =>
        Error.Validation(
            "ObligationAlert.CannotUpdateStatus",
            $"Cannot update alert status to '{status}'. Only 'UnderReview' or 'Resolved' transitions are supported.");
}