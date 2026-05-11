using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.WithdrawPayment;

internal sealed class WithdrawPaymentCommandHandler(
    IPaymentDueRepository paymentDueRepository,
    IPaymentRepository paymentRepository,
    IAlimonyRepository alimonyRepository,
    IParentRepository parentRepository,
    IStripeGateway stripeGateway)
    : ICommandHandler<WithdrawPaymentCommand>
{
    public async Task<Result> Handle(
        WithdrawPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymentDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymentDueId);

        if (!paymentDue.IsPaid)
            return WithdrawalErrors.PaymentNotPaid;

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId, cancellationToken)
            ?? throw new InvalidOperationException("Payment exists but alimony record is missing.");

        if (alimony.RecipientId != request.ParentId)
            return WithdrawalErrors.Unauthorized;

        if (paymentDue.WithdrawalStatus is WithdrawalStatus.Completed or WithdrawalStatus.Processing)
            return WithdrawalErrors.AlreadyWithdrawn;

        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);
        
        var payment = await paymentRepository.GetByPaymentDueIdAsync(paymentDue.Id, cancellationToken)
            ?? throw new InvalidOperationException("Payment record is missing for a paid item.");

        if (string.IsNullOrWhiteSpace(payment.PaymentIntentId))
            return WithdrawalErrors.MissingPaymentReference;

        var result = await stripeGateway.SendPayoutAsync(
            parent!, 
            paymentDue, 
            payment.PaymentIntentId!, 
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error;

        paymentDue.MarkWithdrawalStatusAs(WithdrawalStatus.Processing);
        paymentDueRepository.Update(paymentDue);

        return Result.Success;
    }
}