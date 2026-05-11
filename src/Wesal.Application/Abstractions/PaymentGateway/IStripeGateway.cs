using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.PaymentGateway;

public interface IStripeGateway
{
    Task<Result<string>> CreatePaymentIntentAsync(
        Parent? payerParent,
        PaymentDue paymentDue,
        CancellationToken cancellationToken = default);

    Task<Result<string>> CreateCustomerAsync(
        Parent parent,
        CancellationToken cancellationToken = default);

    Task<Result<string>> CreateConnectAccountAsync(
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
        string paymentIntentId,
        CancellationToken cancellationToken = default);

    Task DeleteAccount(string accountId);
    
    Task DeleteCustomer(string customerId);
}