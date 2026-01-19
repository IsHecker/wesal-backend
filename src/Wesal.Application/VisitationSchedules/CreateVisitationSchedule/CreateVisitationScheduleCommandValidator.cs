using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

public sealed class CreateVisitationScheduleCommandValidator : AbstractValidator<CreateVisitationScheduleCommand>
{
    public CreateVisitationScheduleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.ParentId).NotEmpty();
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();

        RuleFor(x => x.StartDayInMonth)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(31)
            .WithMessage("Start day must be between 1 and 31");

        RuleFor(x => x.Frequency)
            .MustBeEnumValue<CreateVisitationScheduleCommand, VisitationFrequency>();

        RuleFor(x => x)
            .Must(HaveValidTimeRange)
            .WithMessage("Start time must be before end time");
    }

    private bool HaveValidTimeRange(CreateVisitationScheduleCommand command) =>
        command.StartTime < command.EndTime;
}