using FluentValidation;

namespace Wesal.Application.Families.ListFamiliesByCourt;

public sealed class ListFamiliesByCourtQueryValidator : AbstractValidator<ListFamiliesByCourtQuery>
{
    public ListFamiliesByCourtQueryValidator()
    {
        RuleFor(x => x.CourtId).NotEmpty();

        RuleFor(x => x.NationalId)
            .NotEmpty()
            .WithMessage($"National ID is required")
            .Length(14)
            .WithMessage($"National ID must equal 14 characters")
            .Matches(@"^[0-9]+$")
            .WithMessage($"National ID must contain only digits")
            .When(x => !string.IsNullOrWhiteSpace(x.NationalId));
    }
}