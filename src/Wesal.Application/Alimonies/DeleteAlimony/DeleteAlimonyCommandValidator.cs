using FluentValidation;
using Wesal.Application.Alimonies.DeleteAlimony;

internal sealed class DeleteAlimonyCommandValidator : AbstractValidator<DeleteAlimonyCommand>
{
    public DeleteAlimonyCommandValidator()
    {
        RuleFor(x => x.AlimonyId)
            .NotEmpty()
            .WithMessage("Alimony ID is required.");
    }
}