using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.WithdrawPayment;

internal sealed class WithdrawPaymentCommandHandler(
    IPaymentDueRepository paymentDueRepository,
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

        if (paymentDue.WithdrawalStatus == WithdrawalStatus.Completed)
            return WithdrawalErrors.AlreadyWithdrawn;

        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);

        return await stripeGateway.SendPayoutAsync(parent!, paymentDue, cancellationToken);
    }
}