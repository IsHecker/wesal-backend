using FluentValidation;

namespace Wesal.Application.Notifications.ListNotificationsByParentQuery;

public sealed class ListNotificationsByParentQueryValidator : AbstractValidator<ListNotificationsByParentQuery>
{
    public ListNotificationsByParentQueryValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("ParentId is required");
    }
}