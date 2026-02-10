using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Visitations;

public static class VisitationErrors
{
    public static Error NotFound(Guid visitationId) =>
        Error.NotFound("Visitation.NotFound", $"Visitation with ID '{visitationId}' not found");

    public static Error CannotCompleteBeforeEndTime(DateTime endAt) =>
        Error.Validation(
            "Visitation.CannotCompleteBeforeEndTime",
            $"This visitation cannot be completed before it ends at {endAt.TimeOfDay}.");

    public static Error CannotCompleteWithoutBothPartiesCheckedIn =>
        Error.Validation(
            "Visitation.CannotCompleteWithoutBothPartiesCheckedIn",
            $"This visitation cannot be completed without both parties has checked-in.");

    public static Error CompleteWindowExpired(DateTime endAt, int gracePeriodMinutes) =>
        Error.Validation(
            "Visitation.CompleteWindowExpired",
            $"This visitation can no longer be completed. Completion is from {endAt.TimeOfDay} until {endAt.AddMinutes(gracePeriodMinutes).TimeOfDay}.");

    public static Error CheckInTooLate(DateTime startAt, int gracePeriodMinutes) =>
        Error.Validation(
            "Visitation.CheckInTooLate",
            $"Check-in is no longer allowed. Check-in is from {startAt.TimeOfDay} until {startAt.AddMinutes(gracePeriodMinutes).TimeOfDay}.");

    public static Error NationalIdMismatch =>
        Error.Validation(
            "Visitation.NationalIdMismatch",
            "This visitation is not scheduled for this national id");

    public static Error LocationMismatch =>
        Error.Forbidden(
            "Visitation.LocationMismatch",
            "This visitation is not scheduled at your location");

    public static Error NotScheduledForToday =>
        Error.Validation(
            "Visitation.NotScheduledForToday",
            "This visitation is not scheduled for today");

    public static Error AlreadyCheckedIn =>
        Error.Validation(
            "Visitation.AlreadyCheckedIn",
            "Already checked-in.");
}
