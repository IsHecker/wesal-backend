using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.PaymentGateway;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentProcessing.CreateOnboardingSession;

internal sealed class CreateOnboardingSessionCommandHandler(
    IParentRepository parentRepository,
    IStripeGateway stripeGateway)
    : ICommandHandler<CreateOnboardingSessionCommand, SessionResponse>
{
    public async Task<Result<SessionResponse>> Handle(
        CreateOnboardingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);

        var sessionResult = await stripeGateway.CreateOnboardingSessionAsync(
            parent.StripeConnectAccountId,
            request.RefreshUrl,
            request.ReturnUrl,
            cancellationToken);

        if (sessionResult.IsFailure)
            return sessionResult.Error;

        return new SessionResponse(sessionResult.Value);
    }
}