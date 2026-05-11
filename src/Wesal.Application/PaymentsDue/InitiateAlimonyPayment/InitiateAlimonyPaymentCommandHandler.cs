using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.PaymentGateway;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.InitiateAlimonyPayment;

internal sealed class InitiateAlimonyPaymentCommandHandler(
    IParentRepository parentRepository,
    IAlimonyRepository alimonyRepository,
    IPaymentDueRepository paymentDueRepository,
    IStripeGateway stripeGateway)
    : ICommandHandler<InitiateAlimonyPaymentCommand, PaymentIntentResponse>
{
    public async Task<Result<PaymentIntentResponse>> Handle(
        InitiateAlimonyPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymentDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymentDueId);
 
        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId, cancellationToken);
        if (alimony is null)
            return PaymentDueErrors.AlimonyNotFound(paymentDue.AlimonyId);

        if (alimony.PayerId != request.ParentId)
            return PaymentErrors.Unauthorized("You are not authorized to make this payment");
 
        if (paymentDue.IsPaid)
            return PaymentDueErrors.IsAlreadyPaid;
 
        if (paymentDue.IsDueDatePassed)
            return PaymentDueErrors.DueDatePassed;
 
        var payerParent = await parentRepository.GetByIdAsync(alimony.PayerId, cancellationToken);
        if (payerParent is null)
            return ParentErrors.NotFound(alimony.PayerId);

        var result = await stripeGateway.CreatePaymentIntentAsync(
            payerParent,
            paymentDue,
            cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return new PaymentIntentResponse(result.Value);
    }
}