using FluentValidation;

namespace Wesal.Application.UserDevices.UnregisterDevice;

public sealed class UnregisterDeviceCommandValidator : AbstractValidator<UnregisterDeviceCommand>
{
    public UnregisterDeviceCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.DeviceToken)
            .NotEmpty()
            .WithMessage("DeviceToken is required");
    }
}