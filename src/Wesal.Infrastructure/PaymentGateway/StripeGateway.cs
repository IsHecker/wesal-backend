using Stripe.Checkout;
using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class StripeGateway(CurrencyConverter currencyConverter) : IStripeGateway
{
    public async Task<string> CreateCheckoutSessionAsync(
        Parent payerParent,
        PaymentDue paymentDue,
        string successUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default)
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            Customer = payerParent.StripeCustomerId,
            PaymentMethodTypes = ["card"],
            SavedPaymentMethodOptions = new SessionSavedPaymentMethodOptionsOptions
            {
                PaymentMethodSave = "enabled"
            },
            LineItems = [new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    UnitAmount = paymentDue.Amount,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Payment Due",
                        Description = $"Payment due on {paymentDue.DueDate:MMMM dd, yyyy}"
                    }
                },
                Quantity = 1
            }],
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            PaymentIntentData = new SessionPaymentIntentDataOptions()
            .WithKey(MetadataKeys.PayerId, payerParent.Id)
            .WithKey(MetadataKeys.AlimonyId, paymentDue.AlimonyId)
            .WithKey(MetadataKeys.PaymentDueId, paymentDue.Id)
        };

        var session = await new SessionService()
            .CreateAsync(options, cancellationToken: cancellationToken);

        return session.Url;
    }

    public async Task<string> CreateCustomerAsync(Parent parent, CancellationToken cancellationToken = default)
    {
        var customer = await new Stripe.CustomerService().CreateAsync(new Stripe.CustomerCreateOptions
        {
            Name = parent.FullName,
            Email = parent.Email
        }, cancellationToken: cancellationToken);

        return customer.Id;
    }

    public async Task<string> CreateConnectAccountAsync(
        Parent parent,
        CancellationToken cancellationToken = default)
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
            return Error.Failure("Stripe.ConnectFailed", ex.Message);
        }
    }

    public async Task<Result> SendPayoutAsync(
        Parent parent,
        PaymentDue paymentDue,
        CancellationToken cancellationToken = default)
    {
        var convertedAmountInUsd = await currencyConverter.EgpToUsdAsync(paymentDue.Amount);
        try
        {
            if (!parent.IsPayoutMethodConfigured)
                return Error.Validation(
                    "Payout.NotConfigured",
                    "Payout is not configured. Please complete your payout setup to receive withdrawals.");

            var options = new Stripe.TransferCreateOptions
            {
                Amount = convertedAmountInUsd,
                Currency = "usd",
                Destination = parent.StripeConnectAccountId,
                Metadata = new Dictionary<string, string>
                {
                    [MetadataKeys.PaymentDueId.Key] = paymentDue.Id.ToString(),
                    [MetadataKeys.RecipientId.Key] = parent.Id.ToString()
                }
            };

            var transfer = await new Stripe.TransferService()
                .CreateAsync(options, cancellationToken: cancellationToken);

            var payoutOptions = new Stripe.PayoutCreateOptions
            {
                Amount = paymentDue.Amount,
                Currency = "egp",
                Metadata = new Dictionary<string, string>
                {
                    [MetadataKeys.PaymentDueId.Key] = paymentDue.Id.ToString(),
                    [MetadataKeys.RecipientId.Key] = parent.Id.ToString()
                }
            };

            await new Stripe.PayoutService().CreateAsync(
                payoutOptions,
                new Stripe.RequestOptions
                {
                    StripeAccount = parent.StripeConnectAccountId
                },
                cancellationToken);

            return Result.Success;
        }
        catch (Stripe.StripeException ex)
        {
            return Error.Failure(
                $"Stripe.Transfer.{ex.StripeError.Code}",
                ex.StripeError.Message);
        }
    }

    public async Task DeleteAccount(string accountId)
    {
        var service = new Stripe.AccountService();
        var deleted = await service.DeleteAsync(accountId);
    }
}