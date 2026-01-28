using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.CourtCases;

namespace Wesal.Domain.Entities.Alimonies;

public sealed class Alimony : Entity
{
    public Guid CourtId { get; private set; }
    public Guid CourtCaseId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid PayerId { get; private set; }
    public Guid RecipientId { get; private set; }

    public long Amount { get; private set; }

    public AlimonyFrequency Frequency { get; private set; }

    public DateOnly StartDate { get; private set; }

    // When this obligation ends (could be years later)
    public DateOnly EndDate { get; private set; }

    public DateOnly? LastGeneratedDate { get; private set; } = null;

    private Alimony() { }

    public static Alimony Create(
        CourtCase courtCase,
        Guid payerId,
        Guid recipientId,
        long amount,
        AlimonyFrequency frequency,
        DateOnly startDate,
        DateOnly endDate)
    {
        return new Alimony
        {
            CourtId = courtCase.CourtId,
            FamilyId = courtCase.FamilyId,
            CourtCaseId = courtCase.Id,
            PayerId = payerId,
            RecipientId = recipientId,
            Amount = amount,
            Frequency = frequency,
            StartDate = startDate,
            EndDate = endDate,
        };
    }

    public void UpdateLastGeneratedDate(DateOnly visitationDate)
    {
        if (LastGeneratedDate.HasValue && visitationDate < LastGeneratedDate.Value)
            throw new InvalidOperationException();

        LastGeneratedDate = visitationDate;
    }

    public int GetFrequencyInDays(DateOnly targetDate)
    {
        return Frequency switch
        {
            AlimonyFrequency.Daily => 1,
            AlimonyFrequency.Weekly => 7,
            AlimonyFrequency.Monthly => DateTime.DaysInMonth(targetDate.Year, targetDate.Month),
            AlimonyFrequency.Yearly => targetDate.AddYears(1).DayNumber - targetDate.DayNumber,
            _ => throw new NotImplementedException()
        };
    }
}