using FluentValidation;

namespace Wesal.Application.VisitationLocations.ListVisitationLocations;

public sealed class ListVisitationLocationsQueryValidator : AbstractValidator<ListVisitationLocationsQuery>
{
    public ListVisitationLocationsQueryValidator()
    {
        RuleFor(x => x.CourtId)
            .NotEmpty()
            .WithMessage("Court ID is required");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Name filter cannot exceed 100 characters");
    }
}