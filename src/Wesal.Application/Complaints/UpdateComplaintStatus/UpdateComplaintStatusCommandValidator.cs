using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Complaints;

namespace Wesal.Application.Complaints.UpdateComplaintStatus;

internal sealed class UpdateComplaintStatusCommandValidator : AbstractValidator<UpdateComplaintStatusCommand>
{
    public UpdateComplaintStatusCommandValidator()
    {
        RuleFor(x => x.ComplaintId)
            .NotEmpty()
            .WithMessage("Alert ID is required");

        RuleFor(x => x.CourtId)
            .NotEmpty()
            .WithMessage("Court ID is required");

        RuleFor(x => x.Status)
            .MustBeEnumValue<UpdateComplaintStatusCommand, ComplaintStatus>();

        RuleFor(x => x.ResolutionNotes)
            .NotEmpty()
            .WithMessage("Notes are required when resolving or rejecting a complaint")
            .When(x => x.Status == nameof(ComplaintStatus.Resolved) || x.Status == nameof(ComplaintStatus.Rejected));
    }
}