using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Visitations;

public static class VisitationErrors
{
    public static Error NotFound(Guid visitationId) =>
        Error.NotFound("Visitation.NotFound", $"Visitation with ID '{visitationId}' not found");

    public static Error CannotCompleteBeforeEndTime(TimeOnly endTime) =>
        Error.Validation(
            "Visitation.CannotCompleteBeforeEndTime",
            $"This visitation cannot be completed before it ends at {endTime}.");

    public static Error CompletionWindowExpired(TimeOnly endTime, int gracePeriodMinutes) =>
        Error.Validation(
            "Visitation.CompletionWindowExpired",
            $"This visitation can no longer be completed. Completion is from {endTime} until {endTime.AddMinutes(gracePeriodMinutes)}.");

    public static Error CheckInTooLate(TimeOnly startTime, int gracePeriodMinutes) =>
        Error.Validation(
            "Visitation.CheckInTooLate",
            $"Check-in is no longer allowed. Check-in is from {startTime} until {startTime.AddMinutes(gracePeriodMinutes)}.");

    public static Error LocationMismatch =>
        Error.Forbidden(
            "Visitation.LocationMismatch",
            "This visitation is not scheduled at your location");

    public static Error NotScheduledForToday =>
        Error.Validation(
            "Visitation.NotScheduledForToday",
            "This visitation is not scheduled for today");
}
