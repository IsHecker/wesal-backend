using FluentValidation;
using Wesal.Application.VisitationSchedules.DeleteVisitationSchedule;

internal sealed class DeleteVisitationScheduleCommandValidator
    : AbstractValidator<DeleteVisitationScheduleCommand>
{
    public DeleteVisitationScheduleCommandValidator()
    {
        RuleFor(x => x.ScheduleId)
            .NotEmpty()
            .WithMessage("Schedule ID is required.");
    }
}