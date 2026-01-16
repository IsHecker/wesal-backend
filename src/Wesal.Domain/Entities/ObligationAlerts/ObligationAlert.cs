using Wesal.Domain.DomainEvents;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.ObligationAlerts;

public sealed class ObligationAlert : Entity
{
    public Guid ParentId { get; private set; }
    public Guid CourtId { get; private set; }

    // ID of the record that caused the alert (e.g., VisitId or PaymentId).
    public Guid RelatedEntityId { get; private set; }

    public AlertType Type { get; private set; }
    public string Description { get; private set; } = null!;

    public DateTime TriggeredAt { get; private set; }

    public AlertStatus Status { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public string ResolutionNotes { get; private set; } = null!;

    private ObligationAlert() { }

    public static ObligationAlert Create(
        Guid parentId,
        Guid relatedEntityId,
        Guid courtId,
        AlertType type,
        string description,
        DateTime triggeredAt,
        AlertStatus status,
        string resolutionNotes,
        DateTime? resolvedAt = null)
    {
        return new ObligationAlert
        {
            ParentId = parentId,
            RelatedEntityId = relatedEntityId,
            CourtId = courtId,
            Type = type,
            Description = description,
            TriggeredAt = triggeredAt,
            Status = status,
            ResolvedAt = resolvedAt,
            ResolutionNotes = resolutionNotes,
        };
    }

    public Result UpdateStatus(AlertStatus status, string? resolutionNotes)
    {
        return status switch
        {
            AlertStatus.UnderReview => MarkAsUnderReview(),
            AlertStatus.Resolved => Resolve(resolutionNotes!, DateTime.UtcNow),
            _ => Result.Success
        };
    }

    private Result MarkAsUnderReview()
    {
        if (Status == AlertStatus.Resolved)
            return ObligationAlertErrors.CannotModifyResolved();

        if (Status == AlertStatus.UnderReview)
            return ObligationAlertErrors.AlreadyUnderReview();

        Status = AlertStatus.UnderReview;
        return Result.Success;
    }

    private Result Resolve(string resolutionNotes, DateTime resolvedAt)
    {
        if (Status == AlertStatus.Resolved)
            return ObligationAlertErrors.AlreadyResolved();

        Status = AlertStatus.Resolved;
        ResolutionNotes = resolutionNotes;
        ResolvedAt = resolvedAt;

        return Result.Success;
    }
}