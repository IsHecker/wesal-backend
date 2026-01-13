
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.Services;

public interface IPaymentGatewayService
{
    Task<Result> ProcessPaymentAsync(
        long amount,
        string currency,
        PaymentMethod paymentMethod,
        CancellationToken cancellationToken = default);
}