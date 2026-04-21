using FluentValidation;
using Wesal.Domain.Common;

namespace Wesal.Application.Custodies.CreateCustody;

internal sealed class CreateCustodyCommandValidator : AbstractValidator<CreateCustodyCommand>
{
    public CreateCustodyCommandValidator()
    {
        RuleFor(x => x.CourtId).NotEmpty();
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.CustodialParentId).NotEmpty();

        RuleFor(x => x.StartAt.Date)
            .NotEmpty()
            .GreaterThanOrEqualTo(EgyptTime.Now.Date)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .When(x => x.EndAt.HasValue)
            .WithMessage("Start date must be before end date");
    }

    private bool HaveValidDateRange(CreateCustodyCommand command) =>
        !command.EndAt.HasValue || command.StartAt < command.EndAt.Value;
}