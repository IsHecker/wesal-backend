using FluentValidation;

namespace Wesal.Application.Notifications.GetUnreadCount;

public sealed class GetUnreadCountQueryValidator : AbstractValidator<GetUnreadCountQuery>
{
    public GetUnreadCountQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}