using FluentValidation;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Results;
using Wesal.Application.Extensions;
using Wesal.Domain.Common;

namespace Wesal.Application.CustodyRequests.RespondToCustodyRequest;

internal sealed class RespondToCustodyRequestCommandValidator : AbstractValidator<RespondToCustodyRequestCommand>
{
    public RespondToCustodyRequestCommandValidator()
    {
        RuleFor(x => x.ReasonNote)
            .NotEmpty()
            .When(x => !x.IsAccepted)
            .WithMessage("A reason must be provided when rejecting a custody request.");
    }
}