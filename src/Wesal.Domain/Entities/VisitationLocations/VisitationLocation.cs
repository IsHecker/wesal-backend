using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.VisitationLocations;

public sealed class VisitationLocation : Entity
{
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;

    public int MaxConcurrentVisits { get; private set; }
    public bool IsActive { get; private set; }

    public string? Notes { get; private set; }
    public string? ContactNumber { get; private set; }

    private VisitationLocation() { }

    public static VisitationLocation Create(
        string name,
        string address,
        string governorate,
        int maxConcurrentVisits,
        bool isActive,
        string? notes = null,
        string? contactNumber = null)
    {
        return new VisitationLocation
        {
            Name = name,
            Address = address,
            Governorate = governorate,
            MaxConcurrentVisits = maxConcurrentVisits,
            IsActive = isActive,
            Notes = notes,
            ContactNumber = contactNumber,
        };
    }
}