using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Complaints;

public sealed class Complaint : Entity
{
    public Guid CourtId { get; private set; }
    public Guid ReporterId { get; private set; }
    public Guid? DocumentId { get; private set; }

    public ComplaintType Type { get; private set; }
    public string Description { get; private set; } = null!;

    public ComplaintStatus Status { get; private set; }
    public DateTime FiledAt { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public string? ResolutionNotes { get; private set; }

    private Complaint() { }

    public static Complaint Create(
        Guid courtId,
        Guid reporterId,
        ComplaintType type,
        string description,
        Guid? documentId = null)
    {
        return new Complaint
        {
            CourtId = courtId,
            ReporterId = reporterId,
            DocumentId = documentId,
            Type = type,
            Description = description,
            FiledAt = EgyptTime.Now,
            Status = ComplaintStatus.Pending
        };
    }

    public Result UpdateStatus(ComplaintStatus status, string? resolutionNotes)
    {
        return status switch
        {
            ComplaintStatus.UnderReview => MarkAsUnderReview(),
            ComplaintStatus.Resolved => MarkAsResolved(resolutionNotes!, EgyptTime.Now),
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
        ResolutionNotes = resolutionNotes;
        ResolvedAt = resolvedAt;

        return Result.Success;
    }
}