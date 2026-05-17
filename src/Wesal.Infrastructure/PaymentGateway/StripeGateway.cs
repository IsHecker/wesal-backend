using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Domain.Entities.Parents;

using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class StripeGateway(StripeOptions stripeOptions) : IStripeGateway
{
    public async Task<Result<string>> CreatePaymentIntentAsync(
        Parent? payerParent,
        PaymentDue paymentDue,
        CancellationToken cancellationToken = default)
    {
        if (payerParent is null)
            return Error.Failure("Payment.PayerNotFound", "The parent record for this payment was not found.");

        try
        {
            var options = new Stripe.PaymentIntentCreateOptions
            {
                Amount = paymentDue.Amount,
                Currency = "egp",
                Customer = payerParent.StripeCustomerId,
                TransferGroup = paymentDue.Id.ToString(),
                AutomaticPaymentMethods = new Stripe.PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            }
            .WithKey(MetadataKeys.PayerId, payerParent.Id)
            .WithKey(MetadataKeys.AlimonyId, paymentDue.AlimonyId)
            .WithKey(MetadataKeys.PaymentDueId, paymentDue.Id);

            var service = new Stripe.PaymentIntentService();
            var intent = await service.CreateAsync(options, cancellationToken: cancellationToken);

            return intent.ClientSecret;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Problem(
                $"Stripe.PaymentIntent.{ex.StripeError?.Code ?? "Unknown"}",
                ex.StripeError?.Message ?? ex.Message);
        }
    }

    public async Task<Result<string>> CreateCustomerAsync(Parent parent, CancellationToken cancellationToken = default)
    {
        if (!stripeOptions.AllowCustomerCreation)
            return string.Empty;

        try
        {
            var customer = await new Stripe.CustomerService().CreateAsync(new Stripe.CustomerCreateOptions
            {
                Name = parent.FullName,
                Email = parent.Email
            }, cancellationToken: cancellationToken);

            return customer.Id;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Problem("Stripe.CustomerCreationFailed", ex.Message);
        }
    }

    public async Task<Result<string>> CreateConnectAccountAsync(
        Parent parent,
        CancellationToken cancellationToken = default)
    {
        if (!stripeOptions.AllowAccountCreation)
            return string.Empty;

        try
        {
            var accountOptions = new Stripe.AccountCreateOptions
            {
                Type = "express",
                Country = "EG",
                Email = parent.Email,
                Capabilities = new Stripe.AccountCapabilitiesOptions
                {
                    Transfers = new Stripe.AccountCapabilitiesTransfersOptions { Requested = true }
                },
                TosAcceptance = new Stripe.AccountTosAcceptanceOptions
                {
                    ServiceAgreement = "recipient"
                },
                Metadata = new Dictionary<string, string>
                {
                    [MetadataKeys.ParentId.Key] = parent.Id.ToString()
                }
            };

            var account = await new Stripe.AccountService()
                .CreateAsync(accountOptions, cancellationToken: cancellationToken);

            return account.Id;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Problem("Stripe.AccountCreationFailed", ex.Message);
        }
    }

    public async Task<Result<string>> CreateOnboardingSessionAsync(
        string stripeConnectAccountId,
        string refreshUrl,
        string returnUrl,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var accountLinkOptions = new Stripe.AccountLinkCreateOptions
            {
                Account = stripeConnectAccountId,
                RefreshUrl = $"{refreshUrl}?refresh=true",
                ReturnUrl = $"{returnUrl}?success=true",
                Type = "account_onboarding"
            };

            var accountLink = await new Stripe.AccountLinkService()
                .CreateAsync(accountLinkOptions, cancellationToken: cancellationToken);

            return accountLink.Url;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Problem("Stripe.ConnectFailed", ex.Message);
        }
    }

    public async Task<Result> SendPayoutAsync(
        Parent parent,
        PaymentDue paymentDue,
        string paymentIntentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!parent.IsPayoutMethodConfigured)
                return Error.Validation(
                    "Payout.NotConfigured",
                    "Payout is not configured. Please complete your payout setup to receive withdrawals.");

            // Fetch the actual settled amount from Stripe to ensure 100% accuracy and zero loss
            var service = new Stripe.PaymentIntentService();
            var paymentIntent = await service.GetAsync(
                paymentIntentId,
                new Stripe.PaymentIntentGetOptions { Expand = ["latest_charge.balance_transaction"] },
                cancellationToken: cancellationToken);

            if (paymentIntent.LatestCharge?.BalanceTransaction is null)
                return WithdrawalErrors.PaymentNotReady;

            var settledAmount = paymentIntent.LatestCharge.BalanceTransaction.Amount;
            var settlementCurrency = paymentIntent.LatestCharge.BalanceTransaction.Currency;

            var options = new Stripe.TransferCreateOptions
            {
                Amount = settledAmount,
                Currency = settlementCurrency,
                Destination = parent.StripeConnectAccountId,
                SourceTransaction = paymentIntent.LatestCharge.Id,
                TransferGroup = paymentDue.Id.ToString(),
                Metadata = new Dictionary<string, string>
                {
                    [MetadataKeys.PaymentDueId.Key] = paymentDue.Id.ToString(),
                    [MetadataKeys.RecipientId.Key] = parent.Id.ToString()
                }
            };

            var transfer = await new Stripe.TransferService()
                .CreateAsync(options, cancellationToken: cancellationToken);

            // var payoutOptions = new Stripe.PayoutCreateOptions
            // {
            //     Amount = settledAmount,
            //     Currency = settlementCurrency,
            //     Metadata = new Dictionary<string, string>
            //     {
            //         [MetadataKeys.PaymentDueId.Key] = paymentDue.Id.ToString(),
            //         [MetadataKeys.RecipientId.Key] = parent.Id.ToString()
            //     }
            // };

            // await new Stripe.PayoutService().CreateAsync(
            //     payoutOptions,
            //     new Stripe.RequestOptions
            //     {
            //         StripeAccount = parent.StripeConnectAccountId
            //     },
            //     cancellationToken);

            return Result.Success;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Problem(
                $"Stripe.Transfer.{ex.StripeError?.Code ?? "Unknown"}",
                ex.StripeError?.Message ?? ex.Message);
        }
    }

    public async Task DeleteAccount(string accountId)
    {
        var service = new Stripe.AccountService();
        var deleted = await service.DeleteAsync(accountId);
    }

    public async Task DeleteCustomer(string customerId)
    {
        var service = new Stripe.CustomerService();
        await service.DeleteAsync(customerId);
    }
}
