using FluentValidation;

namespace Wesal.Application.CustodyRequests.ProcessCustodyRequest;

public sealed class ProcessCustodyRequestCommandValidator : AbstractValidator<ProcessCustodyRequestCommand>
{
    public ProcessCustodyRequestCommandValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();

        RuleFor(x => x.DecisionNote)
            .NotEmpty()
            .WithMessage("Decision note is required")
            .MaximumLength(1000)
            .WithMessage("Decision note cannot exceed 1000 characters")
            .MinimumLength(10)
            .WithMessage("Decision note must be at least 10 characters");
    }
}