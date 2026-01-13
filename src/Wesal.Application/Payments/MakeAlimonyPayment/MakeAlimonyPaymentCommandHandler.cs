using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

public sealed class MakeAlimonyPaymentCommandHandler(
    IAlimonyRepository alimonyRepository,
    IPaymentRepository paymentRepository,
    IPaymentGatewayService paymentGateway,
    INotificationService notificationService,
    IUnitOfWork unitOfWork) : ICommandHandler<MakeAlimonyPaymentCommand, Guid>
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

        var paymentMethod = request.PaymentMethod.ToEnum<PaymentMethod>();

        // TODO: Process payment using by a payment gateway.
        var gatewayResult = await paymentGateway.ProcessPaymentAsync(
            request.Amount,
            alimony.Currency,
            paymentMethod,
            cancellationToken);

        if (gatewayResult.IsFailure)
        {
            var failedPayment = Payment.Create(
                alimony.Id,
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
            request.Amount,
            PaymentStatus.Completed,
            paymentMethod,
            "ReceiptUrl",
            DateTime.UtcNow);

        await paymentRepository.AddAsync(payment, cancellationToken);

        // TODO: Record payment

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.SendPaymentConfirmationAsync(
            alimony.PayerId,
            alimony.ReceiverId,
            request.Amount,
            alimony.Currency,
            payment.ReceiptUrl,
            cancellationToken);

        return payment.Id;
    }
}