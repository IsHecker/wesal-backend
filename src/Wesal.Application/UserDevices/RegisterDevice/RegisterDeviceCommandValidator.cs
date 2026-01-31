using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.UserDevices;

namespace Wesal.Application.UserDevices.RegisterDevice;

public sealed class RegisterDeviceCommandValidator : AbstractValidator<RegisterDeviceCommand>
{
    public RegisterDeviceCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.DeviceToken)
            .NotEmpty()
            .WithMessage("DeviceToken is required")
            .MaximumLength(500)
            .WithMessage("DeviceToken must not exceed 500 characters");

        RuleFor(x => x.Platform)
            .MustBeEnumValue<RegisterDeviceCommand, DevicePlatform>();
    }
}