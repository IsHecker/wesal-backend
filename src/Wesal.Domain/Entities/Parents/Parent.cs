using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Parents;

public sealed class Parent : Entity, IHasUserId
{
    public Guid UserId { get; private set; }
    public Guid CourtId { get; private set; }

    public bool IsFather { get; private set; }

    public string? Email { get; private set; }

    public string NationalId { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    public string Gender { get; private set; } = null!;
    public string? Job { get; private set; }
    public string Address { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public int ViolationCount { get; private set; }

    public string StripeCustomerId { get; private set; } = null!;
    public string StripeConnectAccountId { get; private set; } = null!;
    public bool IsOnboardingComplete { get; private set; }

    public bool IsPayoutMethodConfigured =>
        StripeConnectAccountId is not null && IsOnboardingComplete;

    private Parent() { }

    public static Parent Create(
        Guid userId,
        Guid courtId,
        bool isFather,
        string nationalId,
        string fullName,
        DateOnly birthDate,
        string gender,
        string address,
        string phone,
        string? job,
        string? email)
    {
        return new Parent
        {
            UserId = userId,
            CourtId = courtId,
            IsFather = isFather,
            NationalId = nationalId,
            FullName = fullName,
            BirthDate = birthDate,
            Gender = gender,
            Job = job,
            Address = address,
            Phone = phone,
            Email = email,
            IsOnboardingComplete = false
        };
    }

    public void RecordViolation() => ViolationCount++;
    public void ResetViolationCount() => ViolationCount = 0;

    public void UpdateProfile(
        string? email,
        string? job,
        string address,
        string phone)
    {
        Email = email;
        Job = job;
        Address = address;
        Phone = phone;
    }

    public void SetupStripe(string stripeCustomerId, string stripeConnectAccountId)
    {
        StripeCustomerId = stripeCustomerId;
        StripeConnectAccountId = stripeConnectAccountId;
    }

    public void CompleteOnboarding() => IsOnboardingComplete = true;
}