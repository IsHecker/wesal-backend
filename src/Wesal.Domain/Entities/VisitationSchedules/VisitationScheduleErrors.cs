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
}