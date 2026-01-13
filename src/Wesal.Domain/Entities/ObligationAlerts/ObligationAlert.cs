using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Domain.Entities.ObligationAlerts;

public sealed class ObligationAlert : Entity
{
    public Guid ParentId { get; private set; }
    public Guid CourtCaseId { get; private set; }

    // ID of the record that caused the alert (e.g., VisitId or PaymentId).
    public Guid RelatedEntityId { get; private set; }
    public Guid CourtId { get; private set; }

    public AlertType Type { get; private set; }
    public string Description { get; private set; } = null!;

    public DateTime TriggeredAt { get; private set; }

    public AlertStatus Status { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public string ResolutionNotes { get; private set; } = null!;

    public Parent Parent { get; private set; } = null!;
    public CourtCase CourtCase { get; private set; } = null!;
    public FamilyCourt Court { get; private set; } = null!;

    private ObligationAlert() { }

    public static ObligationAlert Create(
        Guid parentId,
        Guid courtCaseId,
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
            CourtCaseId = courtCaseId,
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
}
