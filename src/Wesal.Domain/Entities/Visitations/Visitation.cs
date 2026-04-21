using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Visitations;

public sealed class Visitation : Entity
{
    public Guid VisitationScheduleId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid NonCustodialParentId { get; private set; }
    public string NonCustodialNationalId { get; private set; } = null!;
    public string CompanionNationalId { get; private set; } = null!;
    public Guid LocationId { get; private set; }
    public Guid? VerifiedById { get; private set; }

    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }

    public VisitationStatus Status { get; private set; }

    public VisitationAttendance Attendance { get; private set; } = new();

    public bool IsNotified { get; private set; }

    public VisitationSchedule VisitationSchedule { get; private set; } = null!;

    private Visitation() { }

    public static Visitation Create(
        VisitationSchedule schedule,
        DateTime visitationAt)
    {
        return new Visitation
        {
            FamilyId = schedule.FamilyId,
            NonCustodialParentId = schedule.NonCustodialParentId,
            NonCustodialNationalId = schedule.NonCustodialNationalId,
            CompanionNationalId = schedule.CustodialNationalId,
            LocationId = schedule.LocationId,
            VisitationScheduleId = schedule.Id,
            StartAt = visitationAt,
            EndAt = visitationAt.Date + schedule.EndTime.ToTimeSpan(),
            IsNotified = false
        };
    }

    public Result CheckIn(
        Guid staffLocationId,
        string nationalId,
        int gracePeriodMinutes,
        IEnumerable<Guid>? attendingChildrenIds = null)
    {
        attendingChildrenIds ??= [];
        if (LocationId != staffLocationId)
            return VisitationErrors.LocationMismatch;

        if (EgyptTime.Now.Date != StartAt.Date)
            return VisitationErrors.NotScheduledForToday;

        if (!IsWithinTimeWindow(StartAt, gracePeriodMinutes))
            return VisitationErrors.CheckInTooLate(StartAt, gracePeriodMinutes);

        if (nationalId == CompanionNationalId)
        {
            if (Attendance.IsCompanionCheckedIn && attendingChildrenIds.All(c => Attendance.AttendedChildrenIds.Contains(c)))
                return VisitationErrors.AlreadyCheckedIn;

            if (!Attendance.IsCompanionCheckedIn)
                Attendance.CompanionCheckedInAt = EgyptTime.Now;

            Attendance.AttendedChildrenIds = attendingChildrenIds.ToList();
        }
        else if (nationalId == NonCustodialNationalId)
        {
            if (Attendance.IsNonCustodialCheckedIn)
                return VisitationErrors.AlreadyCheckedIn;

            Attendance.NonCustodialCheckedInAt = EgyptTime.Now;
        }
        else
            return VisitationErrors.NationalIdMismatch;

        if (Attendance.AreBothCheckedIn)
        {
            Status = VisitationStatus.InProgress;
            return Result.Success;
        }

        Status = VisitationStatus.PartiallyCheckedIn;
        return Result.Success;
    }

    public Result CheckOut(VisitCenterStaff staff, string nationalId, int checkOutGraceMinutes, int checkInGraceMinutes)
    {
        if (EgyptTime.Now < StartAt.AddMinutes(checkInGraceMinutes))
        {
            return VisitationErrors.CannotCheckOutEarlyBeforeStartTimeFinish(StartAt.AddMinutes(checkInGraceMinutes));
        }

        if (LocationId != staff.LocationId)
            return VisitationErrors.LocationMismatch;

        if (EgyptTime.Now.Date != StartAt.Date)
            return VisitationErrors.NotScheduledForToday;

        if (Attendance.AreBothCheckedIn)
        {
            var transitionResult = StatusTransition.Validate(Status,
            VisitationStatus.InProgress,
            VisitationStatus.Completed);

            if (transitionResult.IsFailure)
                return transitionResult.Error;

            if (EgyptTime.Now < EndAt)
                return VisitationErrors.CannotCompleteBeforeEndTime(EndAt);

            if (!IsWithinTimeWindow(EndAt, checkOutGraceMinutes))
                return VisitationErrors.CompleteWindowExpired(EndAt, checkOutGraceMinutes);
        }

        if (nationalId == CompanionNationalId)
        {
            if (Attendance.IsCompanionCheckedOut)
                return VisitationErrors.AlreadyCheckedOut;

            Attendance.CompanionCheckedOutAt = EgyptTime.Now;
        }
        else if (nationalId == NonCustodialNationalId)
        {
            if (Attendance.IsNonCustodialCheckedOut)
                return VisitationErrors.AlreadyCheckedOut;

            Attendance.NonCustodialCheckedOutAt = EgyptTime.Now;
        }
        else
            return VisitationErrors.NationalIdMismatch;

        if (!Attendance.AreBothCheckedOut)
            return Result.Success;

        Status = VisitationStatus.Completed;

        Attendance.CompletedAt = EgyptTime.Now;
        VerifiedById = staff.Id;

        return Result.Success;
    }

    public void SetCompanion(string nationalId) => CompanionNationalId = nationalId;

    public void MarkAsMissed() => Status = VisitationStatus.Missed;
    public void MarkAsPartiallyCompleted() => Status = VisitationStatus.PartiallyCompleted;

    public void MarkAsOverstayedVisit()
    {
        if (!Attendance.IsNonCustodialCheckedOut && Attendance.IsNonCustodialCheckedIn)
            Attendance.NonCustodialOverstayed = true;

        if (!Attendance.IsCompanionCheckedOut && Attendance.IsCompanionCheckedIn)
            Attendance.CompanionOverstayed = true;

        if (Attendance.NonCustodialOverstayed && Attendance.CompanionOverstayed)
        {
            Status = VisitationStatus.OverstayedVisit;
            return;
        }
        Status = VisitationStatus.PartiallyCompleted;
    }

    public void MarkAsCompleted() => Status = VisitationStatus.Completed;
    public void MarkAsNotified() => IsNotified = true;

    private Result ValidateTransition(
        Guid staffLocationId,
        VisitationStatus requiredStatus,
        VisitationStatus targetStatus)
    {
        var transitionResult = StatusTransition.Validate(Status, requiredStatus, targetStatus);

        if (transitionResult.IsFailure)
            return transitionResult.Error;

        if (LocationId != staffLocationId)
            return VisitationErrors.LocationMismatch;

        if (EgyptTime.Now.Date != StartAt.Date)
            return VisitationErrors.NotScheduledForToday;

        return Result.Success;
    }

    private static bool IsWithinTimeWindow(DateTime dateTime, int gracePeriodMinutes)
    {
        var timeNow = EgyptTime.Now;
        return timeNow >= dateTime && timeNow <= dateTime.AddMinutes(gracePeriodMinutes);
    }
}