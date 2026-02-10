using FluentValidation;

namespace Wesal.Application.Alimonies.GetAlimonyByCourtCase;

internal sealed class GetAlimonyByCourtCaseQueryValidator : AbstractValidator<GetAlimonyByCourtCaseQuery>
{
    public GetAlimonyByCourtCaseQueryValidator()
    {
        RuleFor(x => x.CourtCaseId)
            .NotEmpty()
            .WithMessage("Court case ID is required.");
    }
}