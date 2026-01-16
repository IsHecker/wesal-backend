using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Payments;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Results;

namespace Wesal.Application.Payments.ListPaymentsByPaymentDue;

internal sealed class ListPaymentsByPaymentDueQueryHandler(
    IPaymentDueRepository paymentDueRepository,
    IWesalDbContext context)
    : IQueryHandler<ListPaymentsByPaymentDueQuery, PagedResponse<PaymentResponse>>
{
    public async Task<Result<PagedResponse<PaymentResponse>>> Handle(
        ListPaymentsByPaymentDueQuery request,
        CancellationToken cancellationToken)
    {
        var paymentDue = await paymentDueRepository.GetByIdAsync(request.PaymetDueId, cancellationToken);
        if (paymentDue is null)
            return PaymentDueErrors.NotFound(request.PaymetDueId);

        var payments = context.Payments
            .Where(payment => payment.PaymentDueId == request.PaymetDueId);

        _ = payments.TryGetNonEnumeratedCount(out var totalCount);

        return await payments
            .OrderByDescending(p => p.PaidAt)
            .Paginate(request.Pagination)
            .Select(payment => new PaymentResponse(
                payment.Id,
                payment.Amount,
                payment.Method.ToString(),
                payment.ReceiptUrl,
                payment.PaidAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}