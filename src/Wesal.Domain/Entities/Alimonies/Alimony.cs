using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Alimonies;

public sealed class Alimony : Entity
{
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid PayerId { get; private set; }
    public Guid RecipientId { get; private set; }

    public long Amount { get; private set; }

    public AlimonyFrequency Frequency { get; private set; }

    // Which day payment is due (1-31 for Monthly, 1-7 for Weekly, etc.)
    public int DueDay { get; private set; }

    public DateTime StartAt { get; private set; }

    // When this obligation ends (could be years later)
    public DateTime EndAt { get; private set; }

    private Alimony() { }

    public static Alimony Create(
        Guid courtCaseId,
        Guid familyId,
        Guid payerId,
        Guid recipientId,
        long amount,
        DateTime startAt,
        DateTime endAt)
    {
        return new Alimony
        {
            FamilyId = familyId,
            CourtCaseId = courtCaseId,
            PayerId = payerId,
            RecipientId = recipientId,
            Amount = amount,
            StartAt = startAt,
            EndAt = endAt,
        };
    }
}