using FluentValidation;
using Wesal.Application.VisitationLocations.DeleteVisitationLocation;

internal sealed class DeleteVisitationLocationCommandValidator
    : AbstractValidator<DeleteVisitationLocationCommand>
{
    public DeleteVisitationLocationCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location ID is required.");
    }
}