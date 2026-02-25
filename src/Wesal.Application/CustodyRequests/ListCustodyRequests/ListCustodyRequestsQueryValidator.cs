using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Users;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

public sealed class ListCustodyRequestsQueryValidator : AbstractValidator<ListCustodyRequestsQuery>
{
    public ListCustodyRequestsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserRole).NotEmpty();

        RuleFor(x => x.Status!)
            .MustBeEnumValue<ListCustodyRequestsQuery, CustodyRequestStatus>()
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .When(x => x.UserRole == UserRole.Parent)
            .WithMessage("Family ID is required for parents to view custody requests.");
    }
}