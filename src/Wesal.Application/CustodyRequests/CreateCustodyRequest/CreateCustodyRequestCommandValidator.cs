using FluentValidation;
using Wesal.Domain.Common;

namespace Wesal.Application.CustodyRequests.CreateCustodyRequest;

public sealed class CreateCustodyRequestCommandValidator : AbstractValidator<CreateCustodyRequestCommand>
{
    public CreateCustodyRequestCommandValidator()
    {
        RuleFor(x => x.ParentId).NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(EgyptTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required")
            .MaximumLength(1000)
            .WithMessage("Reason cannot exceed 1000 characters")
            .MinimumLength(20)
            .WithMessage("Reason must be at least 20 characters");
    }
}