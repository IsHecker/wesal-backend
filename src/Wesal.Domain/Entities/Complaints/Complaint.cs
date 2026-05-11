using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Complaints;

public sealed class Complaint : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid ReporterId { get; private set; }
    public Guid AssignedStaffId { get; private set; }
    public Guid? DocumentId { get; private set; }

    public ComplaintType Type { get; private set; }
    public string Description { get; private set; } = null!;

    public ComplaintStatus Status { get; private set; }
    public DateTime FiledAt { get; private set; }

    public DateTime? ResolvedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }

    public string? Notes { get; private set; }

    public Parent Reporter { get; init; } = null!;
    public CourtStaffs.CourtStaff? AssignedStaff { get; private set; }

    private Complaint() { }

    public static Complaint Create(
        Guid courtId,
        Guid familyId,
        Guid reporterId,
        ComplaintType type,
        string description,
        Guid assignedStaffId,
        Guid? documentId = null)
    {
        return new Complaint
        {
            CourtId = courtId,
            FamilyId = familyId,
            ReporterId = reporterId,
            AssignedStaffId = assignedStaffId,
            DocumentId = documentId,
            Type = type,
            Description = description,
            FiledAt = EgyptTime.Now,
            Status = ComplaintStatus.Pending
        };
    }

    public Result UpdateStatus(ComplaintStatus status, string? notes)
    {
        return status switch
        {
            ComplaintStatus.UnderReview => MarkAsUnderReview(),
            ComplaintStatus.Resolved => MarkAsResolved(notes!, EgyptTime.Now),
            ComplaintStatus.Rejected => MarkAsRejected(notes!, EgyptTime.Now),
            _ => ComplaintErrors.CannotUpdateStatus(status)
        };
    }

    private Result MarkAsUnderReview()
    {
        var result = StatusTransition
            .Validate(Status, ComplaintStatus.Pending, ComplaintStatus.UnderReview);

        if (result.IsFailure)
            return result.Error;

        Status = ComplaintStatus.UnderReview;
        return Result.Success;
    }

    private Result MarkAsResolved(string resolutionNotes, DateTime resolvedAt)
    {
        var result = StatusTransition
            .Validate(Status, ComplaintStatus.UnderReview, ComplaintStatus.Resolved);

        if (result.IsFailure)
            return result.Error;

        Status = ComplaintStatus.Resolved;
        Notes = resolutionNotes;
        ResolvedAt = resolvedAt;

        return Result.Success;
    }

    private Result MarkAsRejected(string rejectionReason, DateTime rejectedAt)
    {
        var result = StatusTransition
            .Validate(Status, [ComplaintStatus.Pending, ComplaintStatus.UnderReview], ComplaintStatus.Rejected);

        if (result.IsFailure)
            return result.Error;

        Status = ComplaintStatus.Rejected;
        Notes = rejectionReason;
        RejectedAt = rejectedAt;

        return Result.Success;
    }
}