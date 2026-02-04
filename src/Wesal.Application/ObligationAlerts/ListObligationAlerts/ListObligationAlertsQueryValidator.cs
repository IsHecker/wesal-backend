using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.ObligationAlerts;

namespace Wesal.Application.ObligationAlerts.ListObligationAlerts;

internal sealed class ListObligationAlertsQueryValidator : AbstractValidator<ListObligationAlertsQuery>
{
    public ListObligationAlertsQueryValidator()
    {
        RuleFor(x => x.CourtId)
            .NotEmpty()
            .WithMessage("Court ID is required");

        RuleFor(x => x.Status!)
            .MustBeEnumValue<ListObligationAlertsQuery, AlertStatus>()
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.ViolationType!)
            .MustBeEnumValue<ListObligationAlertsQuery, ViolationType>()
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }
}