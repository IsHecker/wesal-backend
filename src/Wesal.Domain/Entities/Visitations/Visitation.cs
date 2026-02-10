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

    public DateTime? NonCustodialCheckedInAt { get; private set; }
    public DateTime? CompanionCheckedInAt { get; private set; }
    public DateTime? CompletedAt { get; private set; } = null!;

    public bool IsNotified { get; private set; }

    public bool IsNonCustodialCheckedIn => NonCustodialCheckedInAt.HasValue;
    public bool IsCompanionCheckedIn => CompanionCheckedInAt.HasValue;
    public bool AreBothCheckedIn => IsNonCustodialCheckedIn && IsCompanionCheckedIn;

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

    public Result CheckIn(Guid staffLocationId, string nationalId, int gracePeriodMinutes)
    {
        var validation = ValidateTransition(
            staffLocationId,
            VisitationStatus.Scheduled,
            VisitationStatus.CheckedIn);

        if (validation.IsFailure)
            return validation;

        if (!IsWithinTimeWindow(StartAt, gracePeriodMinutes))
            return VisitationErrors.CheckInTooLate(StartAt, gracePeriodMinutes);

        if (nationalId == CompanionNationalId)
        {
            if (IsCompanionCheckedIn)
                return VisitationErrors.AlreadyCheckedIn;

            CompanionCheckedInAt = EgyptTime.Now;
        }
        else if (nationalId == NonCustodialNationalId)
        {
            if (IsNonCustodialCheckedIn)
                return VisitationErrors.AlreadyCheckedIn;

            NonCustodialCheckedInAt = EgyptTime.Now;
        }
        else
            return VisitationErrors.NationalIdMismatch;

        if (AreBothCheckedIn)
            Status = VisitationStatus.CheckedIn;

        return Result.Success;
    }

    public Result Complete(VisitCenterStaff staff, Visitation visitation, int gracePeriodMinutes)
    {
        var validation = ValidateTransition(
            staff.LocationId,
            VisitationStatus.CheckedIn,
            VisitationStatus.Completed);

        if (validation.IsFailure)
            return validation;

        if (!visitation.AreBothCheckedIn)
            return VisitationErrors.CannotCompleteWithoutBothPartiesCheckedIn;

        if (EgyptTime.Now <= EndAt)
            return VisitationErrors.CannotCompleteBeforeEndTime(EndAt);

        if (!IsWithinTimeWindow(EndAt, gracePeriodMinutes))
            return VisitationErrors.CompleteWindowExpired(EndAt, gracePeriodMinutes);

        CompletedAt = EgyptTime.Now;
        Status = VisitationStatus.Completed;
        VerifiedById = staff.Id;

        return Result.Success;
    }

    public void SetCompanion(string nationalId) => CompanionNationalId = nationalId;

    public void MarkAsMissed() => Status = VisitationStatus.Missed;
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