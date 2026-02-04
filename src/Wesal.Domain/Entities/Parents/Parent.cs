using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Parents;

public sealed class Parent : Entity, IHasUserId
{
    public Guid UserId { get; private set; }
    public Guid CourtId { get; private set; }

    public string? Email { get; private set; }

    public string NationalId { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    public string Gender { get; private set; } = null!;
    public string? Job { get; private set; }
    public string Address { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    public int ViolationCount { get; private set; }

    private Parent() { }

    public static Parent Create(
        Guid userId,
        Guid courtId,
        string nationalId,
        string fullName,
        DateOnly birthDate,
        string gender,
        string address,
        string phone,
        string? job = null,
        string? email = null)
    {
        return new Parent
        {
            UserId = userId,
            CourtId = courtId,
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

    public void RecordViolation() => ViolationCount++;
    public void ResetViolationCount() => ViolationCount = 0;

    public void UpdateProfile(
        string fullName,
        string? email,
        string? job,
        string address,
        string phone)
    {
        FullName = fullName;
        Email = email;
        Job = job;
        Address = address;
        Phone = phone;
    }
}