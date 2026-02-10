using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Compliances;

public sealed class ComplianceMetric : Entity
{
    public Guid CourtId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid ParentId { get; private set; }

    public int TotalVisitationsScheduled { get; private set; }
    public int TotalVisitationsCompleted { get; private set; }
    public int TotalVisitationsMissed { get; private set; }

    public int TotalAlimonyPaymentsDue { get; private set; }
    public int TotalAlimonyPaymentsOnTime { get; private set; }
    public int TotalAlimonyPaymentsOverdue { get; private set; }

    public DateOnly Date { get; private set; }
    public DateTime LastUpdatedAt { get; private set; }

    public int TotalViolations => TotalVisitationsMissed + TotalAlimonyPaymentsDue;

    public float VisitationAttendanceRate =>
        TotalVisitationsScheduled > 0
            ? MathF.Round(TotalVisitationsCompleted / TotalVisitationsScheduled * 100, 2) : 0;

    public float AlimonyOnTimeRate =>
        TotalAlimonyPaymentsDue > 0
            ? MathF.Round((float)TotalAlimonyPaymentsOnTime / TotalAlimonyPaymentsDue * 100, 2) : 0;

    private ComplianceMetric() { }

    public static ComplianceMetric Create(
        Guid courtId,
        Guid familyId,
        Guid parentId,
        DateOnly date)
    {
        return new ComplianceMetric
        {
            CourtId = courtId,
            FamilyId = familyId,
            ParentId = parentId,
            Date = new DateOnly(date.Year, date.Month, 1),
            LastUpdatedAt = EgyptTime.Now
        };
    }

    public void RecordVisitationScheduled()
    {
        TotalVisitationsScheduled++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public void RecordVisitationCompleted()
    {
        TotalVisitationsCompleted++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public void RecordVisitationMissed()
    {
        TotalVisitationsMissed++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public void RecordAlimonyDue()
    {
        TotalAlimonyPaymentsDue++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public void RecordAlimonyPaidOnTime()
    {
        TotalAlimonyPaymentsOnTime++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public void RecordAlimonyOverdue()
    {
        TotalAlimonyPaymentsOverdue++;
        LastUpdatedAt = EgyptTime.Now;
    }

    public float GetViolationRate()
    {
        var totalObligations = TotalVisitationsScheduled + TotalAlimonyPaymentsDue;

        return totalObligations > 0
            ? MathF.Round((float)TotalViolations / totalObligations * 100, 2) : 0;
    }
}