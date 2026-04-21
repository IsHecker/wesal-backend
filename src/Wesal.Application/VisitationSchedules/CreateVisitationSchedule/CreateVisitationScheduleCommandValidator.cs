using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Common;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

public sealed class CreateVisitationScheduleCommandValidator : AbstractValidator<CreateVisitationScheduleCommand>
{
    public CreateVisitationScheduleCommandValidator()
    {
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(EgyptTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
 
        RuleFor(x => x.Frequency)
            .MustBeEnumValue<CreateVisitationScheduleCommand, ScheduleFrequency>();
 
        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Start date must be before end date");
 
        RuleFor(x => x)
            .Must(HaveValidTimeRange)
            .WithMessage("Start time must be before end time");
    }

    private bool HaveValidDateRange(CreateVisitationScheduleCommand command) =>
        command.StartDate < command.EndDate;

    private bool HaveValidTimeRange(CreateVisitationScheduleCommand command) =>
        command.StartTime < command.EndTime;
}