using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

internal sealed class MakeAlimonyPaymentCommandHandler(
    IAlimonyRepository alimonyRepository,
    IPaymentRepository paymentRepository,
    IPaymentDueRepository paymentDueRepository,
    IPaymentGatewayService paymentGateway,
    INotificationService notificationService) : ICommandHandler<MakeAlimonyPaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        MakeAlimonyPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var alimony = await alimonyRepository.GetByIdAsync(request.AlimonyId, cancellationToken);
        if (alimony is null)
            return AlimonyErrors.NotFound(request.AlimonyId);

        if (alimony.PayerId != request.PayerId)
            return PaymentErrors.Unauthorized("You are not authorized to make this payment");

        if (request.Amount != alimony.Amount)
            return PaymentErrors.InvalidAmount(request.Amount, alimony.Amount);

        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymetDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymetDueId);

        if (paymentDue.IsPaid)
            return PaymentDueErrors.IsAlreadyPaid;

        if (paymentDue.IsDueDatePassed)
            return PaymentDueErrors.DueDatePassed;

        var paymentMethod = request.PaymentMethod.ToEnum<PaymentMethod>();

        // TODO: Process payment using by a payment gateway.
        var gatewayResult = await paymentGateway.ProcessPaymentAsync(
            request.Amount,
            paymentMethod,
            cancellationToken);

        if (gatewayResult.IsFailure)
        {
            var failedPayment = Payment.Create(
                alimony.Id,
                request.PaymetDueId,
                request.Amount,
                PaymentStatus.Failed,
                paymentMethod,
                string.Empty,
                DateTime.UtcNow);

            await paymentRepository.AddAsync(failedPayment, cancellationToken);

            return gatewayResult.Error;
        }

        var payment = Payment.Create(
            alimony.Id,
            request.PaymetDueId,
            request.Amount,
            PaymentStatus.Paid,
            paymentMethod,
            "ReceiptUrl",
            DateTime.UtcNow);

        paymentDue.MarkAsPaid(payment.Id);

        await paymentRepository.AddAsync(payment, cancellationToken);
        paymentDueRepository.Update(paymentDue);

        // TODO: Record payment

        await notificationService.SendPaymentConfirmationAsync(
            alimony.PayerId,
            alimony.RecipientId,
            request.Amount,
            payment.ReceiptUrl,
            cancellationToken);

        return payment.Id;
    }
}