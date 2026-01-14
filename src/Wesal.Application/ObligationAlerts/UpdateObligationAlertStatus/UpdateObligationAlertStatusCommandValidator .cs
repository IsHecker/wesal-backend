using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.ObligationAlerts;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

internal sealed class UpdateObligationAlertStatusCommandValidator : AbstractValidator<UpdateObligationAlertStatusCommand>
{
    public UpdateObligationAlertStatusCommandValidator()
    {
        RuleFor(x => x.AlertId)
            .NotEmpty()
            .WithMessage("Alert ID is required");

        RuleFor(x => x.StaffId)
            .NotEmpty()
            .WithMessage("Staff ID is required");

        RuleFor(x => x.Status)
            .MustBeEnumValue<UpdateObligationAlertStatusCommand, AlertStatus>();

        RuleFor(x => x.ResolutionNotes)
            .MaximumLength(1000)
            .WithMessage("Resolution notes cannot exceed 1000 characters")
            .MinimumLength(10)
            .WithMessage("Resolution notes must be at least 10 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ResolutionNotes));
    }
}