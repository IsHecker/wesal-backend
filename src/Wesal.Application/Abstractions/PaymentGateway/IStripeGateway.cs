using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.PaymentGateway;

public interface IStripeGateway
{
    Task<string> CreateCheckoutSessionAsync(
        Parent payerParent,
        PaymentDue paymentDue,
        string successUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default);

    Task<string> CreateCustomerAsync(
        Parent parent,
        CancellationToken cancellationToken = default);

    Task<string> CreateConnectAccountAsync(
        Parent parent,
        CancellationToken cancellationToken = default);

    Task<Result<string>> CreateOnboardingSessionAsync(
        string stripeConnectAccountId,
        string refreshUrl,
        string returnUrl,
        CancellationToken cancellationToken = default);

    Task<Result> SendPayoutAsync(
        Parent parent,
        PaymentDue paymentDue,
        CancellationToken cancellationToken = default);

    Task DeleteAccount(string accountId);
}