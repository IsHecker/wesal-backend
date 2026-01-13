using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Users;

namespace Wesal.Domain.Entities.FamilyCourts;

public sealed class FamilyCourt : Entity
{
    public string Name { get; private set; } = null!;
    public string Governorate { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string ContactInfo { get; private set; } = null!;

    public User User { get; private set; } = null!;
    public ICollection<CourtCase> CourtCases { get; private set; } = [];
    public ICollection<ObligationAlert> ObligationAlerts { get; private set; } = [];

    private FamilyCourt() { }

    public static FamilyCourt Create(string name, string governorate, string address, string contactInfo)
    {
        return new FamilyCourt
        {
            Name = name,
            Governorate = governorate,
            Address = address,
            ContactInfo = contactInfo,
        };
    }
}