using FluentValidation;
using Wesal.Application.Alimonies.GetAlimonyByFamily;

internal sealed class GetAlimonyByFamilyQueryValidator : AbstractValidator<GetAlimonyByFamilyQuery>
{
    public GetAlimonyByFamilyQueryValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");
    }
}