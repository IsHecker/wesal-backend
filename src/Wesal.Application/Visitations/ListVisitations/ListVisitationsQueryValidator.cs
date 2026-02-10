using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Visitations;

namespace Wesal.Application.Visitations.ListVisitations;

internal sealed class ListVisitationsQueryValidator : AbstractValidator<ListVisitationsQuery>
{
    public ListVisitationsQueryValidator()
    {
        RuleFor(x => x.NationalId)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.NationalId))
            .WithMessage("National ID cannot exceed 20 characters");

        RuleFor(x => x.Status!)
            .MustBeEnumValue<ListVisitationsQuery, VisitationStatus>()
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(EgyptTime.Today.AddYears(1))
            .When(x => x.Date.HasValue)
            .WithMessage("Date cannot be more than 1 years in the future");
    }
}