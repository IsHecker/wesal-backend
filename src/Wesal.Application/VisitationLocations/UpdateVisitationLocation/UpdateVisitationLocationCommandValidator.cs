using FluentValidation;
using Wesal.Application.VisitationLocations.CreateVisitationLocation;

namespace Wesal.Application.VisitationLocations.UpdateVisitationLocation;

public sealed class UpdateVisitationLocationCommandValidator : AbstractValidator<UpdateVisitationLocationCommand>
{
    public UpdateVisitationLocationCommandValidator()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty()
            .WithMessage("Staff ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location name is required")
            .MaximumLength(200)
            .WithMessage("Location name cannot exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("Location name must be at least 3 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Governorate)
            .NotEmpty()
            .WithMessage("Governorate is required")
            .MaximumLength(100)
            .WithMessage("Governorate cannot exceed 100 characters");

        RuleFor(x => x.ContactNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
            .WithMessage("Contact number cannot exceed 20 characters")
            .Matches(@"^\+?[0-9\s\-()]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
            .WithMessage("Invalid contact number format");

        RuleFor(x => x.MaxConcurrentVisits)
            .GreaterThan(0)
            .WithMessage("Max visits must be greater than zero")
            .LessThanOrEqualTo(1000)
            .WithMessage("Max visits cannot exceed 1000");

        RuleFor(x => x.OpeningTime)
            .NotEmpty()
            .WithMessage("Opening time is required");

        RuleFor(x => x.ClosingTime)
            .NotEmpty()
            .WithMessage("Closing time is required");

        RuleFor(x => x)
            .Must(HaveValidOperatingHours)
            .WithMessage("Opening time must be before closing time");
    }

    private static bool HaveValidOperatingHours(UpdateVisitationLocationCommand command)
    {
        return command.OpeningTime < command.ClosingTime;
    }
}