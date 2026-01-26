using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.CustodyRequests;

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
    }
}