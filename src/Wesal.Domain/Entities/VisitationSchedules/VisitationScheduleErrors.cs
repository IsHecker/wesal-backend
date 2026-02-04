using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.VisitationSchedules;

public static class VisitationScheduleErrors
{
    public static Error InvalidFrequency(string frequency) =>
        Error.Validation("VisitationSchedule.InvalidFrequency", $"Invalid frequency: '{frequency}'");

    public static Error ParentNotInFamily() =>
        Error.Validation("VisitationSchedule.ParentNotInFamily", "Parent is not part of the specified family");

    public static Error NotFound(Guid id) =>
        Error.NotFound("VisitationSchedule.NotFound", $"Visitation schedule with ID '{id}' not found");

    public static readonly Error HasCompletedVisitations =
        Error.Validation(
            "VisitationSchedule.HasCompletedVisitations",
            "Cannot modify or delete a visitation schedule that has completed visitations. They are legal evidence.");

    public static readonly Error CannotModifyClosedCase =
        Error.Validation(
            "VisitationSchedule.CannotModifyClosedCase",
            "Cannot modify a visitation schedule for a closed court case.");
}