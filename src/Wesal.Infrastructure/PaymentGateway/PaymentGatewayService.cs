using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class PaymentGatewayService : IPaymentGatewayService
{
    public async Task<Result> ProcessPaymentAsync(
        long amount,
        string currency,
        PaymentMethod paymentMethod,
        CancellationToken cancellationToken = default)
    {
        // TODO: Choose and implement gateway.

        return Result.Success;
    }
}