using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

internal sealed class MakeAlimonyPaymentCommandHandler(
    IAlimonyRepository alimonyRepository,
    IPaymentRepository paymentRepository,
    IPaymentDueRepository paymentDueRepository,
    IComplianceMetricsRepository metricsRepository)
    : ICommandHandler<MakeAlimonyPaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        MakeAlimonyPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymetDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymetDueId);

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId, cancellationToken)
            ?? throw new InvalidOperationException();

        if (alimony.PayerId != request.UserId)
            return PaymentErrors.Unauthorized("You are not authorized to make this payment");

        if (paymentDue.IsPaid)
            return PaymentDueErrors.IsAlreadyPaid;

        if (paymentDue.IsDueDatePassed)
            return PaymentDueErrors.DueDatePassed;

        var paymentMethod = request.PaymentMethod.ToEnum<PaymentMethod>();

        // TODO: Process payment using by a payment gateway.

        var payment = Payment.Create(
            alimony.Id,
            request.PaymetDueId,
            alimony.Amount,
            PaymentStatus.Paid,
            paymentMethod,
            "ReceiptUrl",
            DateTime.UtcNow);

        paymentDue.MarkAsPaid(payment.Id);

        await paymentRepository.AddAsync(payment, cancellationToken);
        paymentDueRepository.Update(paymentDue);

        await RecordAlimonyPaidOnTimeAsync(alimony, cancellationToken);

        return payment.Id;
    }

    private async Task RecordAlimonyPaidOnTimeAsync(Alimony alimony, CancellationToken cancellationToken)
    {
        var metrics = await metricsRepository.GetAsync(
            alimony.FamilyId,
            alimony.PayerId,
            DateTime.UtcNow.ToDateOnly(),
            cancellationToken) ?? throw new InvalidOperationException();

        metrics.RecordAlimonyPaidOnTime();
        metricsRepository.Update(metrics);
    }
}