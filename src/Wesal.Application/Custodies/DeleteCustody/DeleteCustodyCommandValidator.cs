using FluentValidation;
using Wesal.Application.Custodies.DeleteCustody;

internal sealed class DeleteCustodyCommandValidator : AbstractValidator<DeleteCustodyCommand>
{
    public DeleteCustodyCommandValidator()
    {
        RuleFor(x => x.CustodyId)
            .NotEmpty()
            .WithMessage("Custody ID is required.");
    }
}