using FluentValidation;

namespace Wesal.Application.Notifications.MarkNotificationAsRead;

public sealed class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty()
            .WithMessage("NotificationId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}