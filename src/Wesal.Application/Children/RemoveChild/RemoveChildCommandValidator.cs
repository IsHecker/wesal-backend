using FluentValidation;
using Wesal.Application.Children.RemoveChild;

internal sealed class RemoveChildCommandValidator : AbstractValidator<RemoveChildCommand>
{
    public RemoveChildCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");

        RuleFor(x => x.ChildId)
            .NotEmpty()
            .WithMessage("Child ID is required.");
    }
}