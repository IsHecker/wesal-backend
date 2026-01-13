using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Domain.Entities.Complaints;

public sealed class Complaint : Entity
{
    public Guid ReporterId { get; private set; }
    public Guid AgainstId { get; private set; }

    public ComplaintType Type { get; private set; }
    public string Description { get; private set; } = null!;

    public string? EvidenceUrl { get; private set; }

    public DateTime FiledAt { get; private set; }
    public ComplaintStatus Status { get; private set; }

    public DateTime? ResolvedAt { get; private set; }

    public string? ResolutionNotes { get; private set; }

    public Parent Reporter { get; private set; } = null!;
    public Parent Against { get; private set; } = null!;

    private Complaint() { }

    public static Complaint Create(
        Guid reporterId,
        Guid againstId,
        ComplaintType type,
        string description,
        DateTime filedAt,
        ComplaintStatus status,
        string? evidenceUrl = null,
        DateTime? resolvedAt = null,
        string? resolutionNotes = null)
    {
        return new Complaint
        {
            ReporterId = reporterId,
            AgainstId = againstId,
            Type = type,
            Description = description,
            EvidenceUrl = evidenceUrl,
            FiledAt = filedAt,
            Status = status,
            ResolvedAt = resolvedAt,
            ResolutionNotes = resolutionNotes,
        };
    }
}