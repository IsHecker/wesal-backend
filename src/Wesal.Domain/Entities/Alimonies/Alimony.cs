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

    public DateOnly StartDate { get; private set; }

    // When this obligation ends (could be years later)
    public DateOnly EndDate { get; private set; }

    public DateOnly? LastGeneratedDate { get; private set; } = null;


    // Which day payment is due (1-31 for Monthly, 1-7 for Weekly, etc.)
    public int StartDayInMonth => StartDate.Day;

    private Alimony() { }

    public static Alimony Create(
        Guid courtCaseId,
        Guid familyId,
        Guid payerId,
        Guid recipientId,
        long amount,
        AlimonyFrequency frequency,
        DateOnly startDate,
        DateOnly endDate)
    {
        return new Alimony
        {
            FamilyId = familyId,
            CourtCaseId = courtCaseId,
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