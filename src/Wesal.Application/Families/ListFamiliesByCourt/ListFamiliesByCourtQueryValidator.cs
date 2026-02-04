using FluentValidation;

namespace Wesal.Application.Families.ListFamiliesByCourt;

public sealed class ListFamiliesByCourtQueryValidator : AbstractValidator<ListFamiliesByCourtQuery>
{
    public ListFamiliesByCourtQueryValidator()
    {
        RuleFor(x => x.CourtId).NotEmpty();
    }
}