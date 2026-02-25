using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.PaymentsDue.ListPaymentsDueByAlimony;

internal sealed class ListPaymentsDueByAlimonyQueryHandler(
    IAlimonyRepository alimonyRepository,
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListPaymentsDueByAlimonyQuery, PagedResponse<PaymentDueResponse>>
{
    public async Task<Result<PagedResponse<PaymentDueResponse>>> Handle(
        ListPaymentsDueByAlimonyQuery request,
        CancellationToken cancellationToken)
    {
        var alimony = await alimonyRepository.GetByIdAsync(request.AlimonyId, cancellationToken);
        if (alimony is null)
            return AlimonyErrors.NotFound;

        var paymentsDue = context.PaymentsDue
            .Where(paymentDue => paymentDue.AlimonyId == request.AlimonyId);

        if (request.UserRole == UserRole.Parent)
        {
            var isParentInFamily = await familyRepository.IsParentInFamilyAsync(
                request.UserId,
                alimony.FamilyId,
                cancellationToken);

            if (!isParentInFamily)
                return FamilyErrors.ParentNotInFamily;
        }

        var totalCount = await paymentsDue.CountAsync(cancellationToken);

        return await paymentsDue
            .OrderBy(p => p.DueDate)
            .Paginate(request.Pagination)
            .Select(paymentDue => new PaymentDueResponse(
                paymentDue.Id,
                paymentDue.AlimonyId,
                paymentDue.Amount,
                paymentDue.DueDate,
                paymentDue.Status.ToString(),
                paymentDue.PaidAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}