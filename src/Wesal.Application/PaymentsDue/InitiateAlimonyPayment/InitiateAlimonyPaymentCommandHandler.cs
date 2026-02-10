using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.PaymentGateway;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.InitiateAlimonyPayment;

internal sealed class InitiateAlimonyPaymentCommandHandler(
    IParentRepository parentRepository,
    IAlimonyRepository alimonyRepository,
    IPaymentDueRepository paymentDueRepository,
    IStripeGateway stripeGateway)
    : ICommandHandler<InitiateAlimonyPaymentCommand, SessionResponse>
{
    public async Task<Result<SessionResponse>> Handle(
        InitiateAlimonyPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymentDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymentDueId);

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId, cancellationToken)
            ?? throw new InvalidOperationException();

        if (alimony.PayerId != request.ParentId)
            return PaymentErrors.Unauthorized("You are not authorized to make this payment");

        if (paymentDue.IsPaid)
            return PaymentDueErrors.IsAlreadyPaid;

        if (paymentDue.IsDueDatePassed)
            return PaymentDueErrors.DueDatePassed;

        var payerParent = await parentRepository.GetByIdAsync(alimony.PayerId, cancellationToken);

        var sessionUrl = await stripeGateway.CreateCheckoutSessionAsync(
            payerParent!,
            paymentDue,
            request.SuccessUrl,
            request.CancelUrl,
            cancellationToken);

        return new SessionResponse(sessionUrl);
    }
}