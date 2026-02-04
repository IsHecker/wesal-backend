using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.ListPaymentsDueByFamily;

internal sealed class ListPaymentsDueByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListPaymentsDueByFamilyQuery, PagedResponse<PaymentDueResponse>>
{
    public async Task<Result<PagedResponse<PaymentDueResponse>>> Handle(
        ListPaymentsDueByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var isFamilyExist = await familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (!isFamilyExist)
            return FamilyErrors.NotFound(request.FamilyId);

        if (request.UserRole == UserRole.Parent)
        {
            var isParentInFamily = await familyRepository.IsParentInFamilyAsync(
                request.UserId,
                request.FamilyId,
                cancellationToken);

            if (!isParentInFamily)
                return FamilyErrors.ParentNotInFamily;
        }

        var paymentsDue = context.PaymentsDue
            .Where(paymentDue => paymentDue.FamilyId == request.FamilyId);

        _ = paymentsDue.TryGetNonEnumeratedCount(out var totalCount);

        return await paymentsDue
            .OrderByDescending(p => p.DueDate)
            .Paginate(request.Pagination)
            .Select(paymentDue => new PaymentDueResponse(
                paymentDue.Id,
                paymentDue.AlimonyId,
                paymentDue.Amount,
                paymentDue.DueDate,
                paymentDue.Status.ToString(),
                paymentDue.PaymentId,
                paymentDue.PaidAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}