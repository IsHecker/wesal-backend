using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.VisitationLocations;

public sealed class VisitationLocation : Entity
{
    public Guid CourtId { get; private set; }

    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;

    public int MaxConcurrentVisits { get; private set; }
    public bool IsActive { get; private set; }

    public string? ContactNumber { get; private set; }
    public TimeOnly OpeningTime { get; private set; }
    public TimeOnly ClosingTime { get; private set; }

    private VisitationLocation() { }

    public static VisitationLocation Create(
        Guid courtId,
        string name,
        string address,
        string governorate,
        int maxConcurrentVisits,
        TimeOnly openingTime,
        TimeOnly closingTime,
        string? contactNumber = null)
    {
        return new VisitationLocation
        {
            CourtId = courtId,
            Name = name,
            Address = address,
            Governorate = governorate,
            ContactNumber = contactNumber,
            MaxConcurrentVisits = maxConcurrentVisits,
            OpeningTime = openingTime,
            ClosingTime = closingTime,
            IsActive = true
        };
    }

    public void Update(
        string name,
        string address,
        string governorate,
        string? contactNumber,
        int maxConcurrentVisits,
        TimeOnly openingTime,
        TimeOnly closingTime)
    {
        Name = name;
        Address = address;
        Governorate = governorate;
        ContactNumber = contactNumber;
        MaxConcurrentVisits = maxConcurrentVisits;
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }
}