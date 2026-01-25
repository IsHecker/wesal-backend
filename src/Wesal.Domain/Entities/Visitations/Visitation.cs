using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Visitations;

public sealed class Visitation : Entity
{
    public Guid FamilyId { get; private set; }
    public Guid ParentId { get; private set; }
    public Guid LocationId { get; private set; }
    public Guid VisitationScheduleId { get; private set; }
    public Guid? VerifiedById { get; private set; } = null!;

    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public DateTime EndAt { get; private set; }

    public VisitationStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; } = null!;
    public DateTime? CheckedInAt { get; private set; } = null;

    private Visitation() { }

    public static Visitation Create(VisitationSchedule schedule, DateOnly visitationDate)
    {
        return new Visitation
        {
            FamilyId = schedule.FamilyId,
            ParentId = schedule.ParentId,
            LocationId = schedule.LocationId,
            VisitationScheduleId = schedule.Id,
            Date = visitationDate,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime
        };
    }

    public Result CheckIn(Guid staffLocationId, int gracePeriodMinutes)
    {
        var validation = ValidateTransition(
            staffLocationId,
            VisitationStatus.Scheduled,
            VisitationStatus.CheckedIn);

        if (validation.IsFailure)
            return validation;

        if (!IsWithinTimeWindow(StartTime, gracePeriodMinutes))
            return VisitationErrors.CheckInTooLate(StartTime, gracePeriodMinutes);

        CheckedInAt = DateTime.UtcNow;
        Status = VisitationStatus.CheckedIn;

        return Result.Success;
    }

    public Result Complete(VisitCenterStaff staff, int gracePeriodMinutes)
    {
        var validation = ValidateTransition(
            staff.LocationId,
            VisitationStatus.CheckedIn,
            VisitationStatus.Completed);

        if (validation.IsFailure)
            return validation;

        if (TimeOnly.FromDateTime(DateTime.UtcNow) <= EndTime)
            return VisitationErrors.CannotCompleteBeforeEndTime(EndTime);

        if (!IsWithinTimeWindow(EndTime, gracePeriodMinutes))
            return VisitationErrors.CompletionWindowExpired(EndTime, gracePeriodMinutes);

        CompletedAt = DateTime.UtcNow;
        Status = VisitationStatus.Completed;
        VerifiedById = staff.Id;

        return Result.Success;
    }

    public void MarkAsMissed() => Status = VisitationStatus.Missed;

    private Result ValidateTransition(
        Guid staffLocationId,
        VisitationStatus requiredStatus,
        VisitationStatus targetStatus)
    {
        var now = DateTime.UtcNow;

        var transitionResult = StatusTransition
            .Validate(Status, requiredStatus, targetStatus);
        if (transitionResult.IsFailure)
            return transitionResult.Error;

        if (LocationId != staffLocationId)
            return VisitationErrors.LocationMismatch;

        var today = DateOnly.FromDateTime(now);
        if (today != Date)
            return VisitationErrors.NotScheduledForToday;

        return Result.Success;
    }

    private static bool IsWithinTimeWindow(TimeOnly time, int gracePeriodMinutes)
    {
        var timeNow = TimeOnly.FromDateTime(DateTime.UtcNow);
        return timeNow >= time && timeNow <= time.AddMinutes(gracePeriodMinutes);
    }
}