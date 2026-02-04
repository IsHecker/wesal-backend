using FluentValidation;

namespace Wesal.Application.Notifications.GetUnreadCount;

public sealed class GetUnreadCountQueryValidator : AbstractValidator<GetUnreadCountQuery>
{
    public GetUnreadCountQueryValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}