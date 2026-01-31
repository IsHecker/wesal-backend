using FluentValidation;

namespace Wesal.Application.Notifications.ListNotificationsByUserQuery;

public sealed class ListNotificationsByUserQueryValidator : AbstractValidator<ListNotificationsByUserQuery>
{
    public ListNotificationsByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}