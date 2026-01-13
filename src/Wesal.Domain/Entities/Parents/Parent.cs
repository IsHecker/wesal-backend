using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Domain.Entities.Parents;

public sealed class Parent : Entity
{
    public string NationalId { get; private set; } = null!;

    public string FullName { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public string Gender { get; private set; } = null!;
    public string? Job { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }

    public string? Email { get; private set; }

    public User User { get; private set; } = null!;
    public ICollection<Family> FamiliesAsFather { get; private set; } = [];
    public ICollection<Family> FamiliesAsMother { get; private set; } = [];
    public ICollection<Custody> CustodiesHeld { get; private set; } = [];
    public ICollection<Visitation> VisitationsAsNonCustodialParent { get; private set; } = [];
    public ICollection<Alimony> PaymentsMade { get; private set; } = [];
    public ICollection<Alimony> PaymentsReceived { get; private set; } = [];
    public ICollection<ObligationAlert> AlertsForViolation { get; private set; } = [];
    public ICollection<Notification> Notifications { get; private set; } = [];
    public ICollection<Complaint> ComplaintsFiled { get; private set; } = [];
    public ICollection<Complaint> ComplaintsAgainst { get; private set; } = [];

    private Parent() { }

    public static Parent Create(
        string nationalId,
        string fullName,
        DateTime birthDate,
        string gender,
        string? job = null,
        string? address = null,
        string? phone = null,
        string? email = null)
    {
        return new Parent
        {
            NationalId = nationalId,
            FullName = fullName,
            BirthDate = birthDate,
            Gender = gender,
            Job = job,
            Address = address,
            Phone = phone,
            Email = email
        };
    }
}