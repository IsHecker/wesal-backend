using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Payments;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Payments.ListPaymentsByFamily;

internal sealed class ListPaymentsByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IAlimonyRepository alimonyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListPaymentsByFamilyQuery, PagedResponse<PaymentResponse>>
{
    public async Task<Result<PagedResponse<PaymentResponse>>> Handle(
        ListPaymentsByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var isFamilyExist = await familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (!isFamilyExist)
            return FamilyErrors.NotFound(request.FamilyId);

        var alimony = await alimonyRepository.GetByFamilyIdAsync(request.FamilyId, cancellationToken);

        if (alimony is null)
            return await Enumerable.Empty<PaymentResponse>().ToPagedResponseAsync(request.Pagination, 0);

        var allPayments = context.Payments
            .Where(payment => payment.AlimonyId == alimony.Id);

        _ = allPayments.TryGetNonEnumeratedCount(out var totalCount);

        return await allPayments
            .OrderByDescending(p => p.PaidAt)
            .Paginate(request.Pagination)
            .Select(p => new PaymentResponse(
                p.Id,
                p.Amount,
                alimony.Currency,
                p.Status.ToString(),
                p.Method.ToString(),
                p.ReceiptUrl,
                p.PaidAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}