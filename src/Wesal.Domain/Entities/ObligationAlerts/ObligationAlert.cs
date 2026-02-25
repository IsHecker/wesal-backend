using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.ObligationAlerts;

public sealed class ObligationAlert : Entity
{
    public Guid ParentId { get; private set; }
    public Guid CourtId { get; private set; }
    public string ParentName { get; private set; } = null!;

    // ID of the record that caused the alert (e.g., VisitId or PaymentId).
    public Guid RelatedEntityId { get; private set; }

    public ViolationType ViolationType { get; private set; }
    public string Description { get; private set; } = null!;

    public DateTime TriggeredAt { get; private set; }

    public AlertStatus Status { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public string? ResolutionNotes { get; private set; }

    private ObligationAlert() { }

    public static ObligationAlert Create(
        Guid courtId,
        Guid parentId,
        string parentName,
        Guid relatedEntityId,
        ViolationType violationType,
        AlertStatus status,
        string description)
    {
        return new ObligationAlert
        {
            ParentId = parentId,
            RelatedEntityId = relatedEntityId,
            CourtId = courtId,
            ParentName = parentName,
            ViolationType = violationType,
            Description = description,
            Status = status,
            TriggeredAt = EgyptTime.Now,
            ResolvedAt = null,
            ResolutionNotes = null,
        };
    }

    public void Activate()
    {
        Status = AlertStatus.Pending;
    }

    public Result UpdateStatus(AlertStatus status, string? resolutionNotes)
    {
        return status switch
        {
            AlertStatus.UnderReview => MarkAsUnderReview(),
            AlertStatus.Resolved => MarkAsResolved(resolutionNotes!, EgyptTime.Now),
            _ => ObligationAlertErrors.CannotUpdateStatus(status)
        };
    }

    private Result MarkAsUnderReview()
    {
        var result = StatusTransition
            .Validate(Status, AlertStatus.Pending, AlertStatus.UnderReview);

        if (result.IsFailure)
            return result.Error;

        Status = AlertStatus.UnderReview;
        return Result.Success;
    }

    private Result MarkAsResolved(string resolutionNotes, DateTime resolvedAt)
    {
        if (Status == AlertStatus.Resolved)
            return ObligationAlertErrors.AlreadyResolved;

        Status = AlertStatus.Resolved;
        ResolutionNotes = resolutionNotes;
        ResolvedAt = resolvedAt;

        return Result.Success;
    }
}