using FluentValidation;
using Wesal.Application.VisitationSchedules.UpdateVisitationSchedule;

internal sealed class UpdateVisitationScheduleCommandValidator
    : AbstractValidator<UpdateVisitationScheduleCommand>
{
    public UpdateVisitationScheduleCommandValidator()
    {
        RuleFor(x => x.ScheduleId)
            .NotEmpty()
            .WithMessage("Schedule ID is required.");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location ID is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.")
            .Must((cmd, endTime) => endTime > cmd.StartTime)
            .WithMessage("End time must be after start time.");
    }
}