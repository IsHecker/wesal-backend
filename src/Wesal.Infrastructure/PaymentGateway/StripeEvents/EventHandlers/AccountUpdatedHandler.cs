using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.AccountUpdated)]
internal sealed class AccountUpdatedHandler(
    IParentRepository parentRepository,
    IUnitOfWork unitOfWork) : StripeEventHandler<Account>
{
    protected override async Task<Result> HandleAsync(Account account)
    {
        var parent = await parentRepository.GetByStripeAccountIdAsync(account.Id);

        if (!parent!.IsOnboardingComplete
            && account.PayoutsEnabled
            && account.ChargesEnabled
            && account.Capabilities.Transfers == "active"
            && account.Requirements.CurrentlyDue.Count == 0
            && account.Requirements.Errors.Count == 0)
        {
            parent.CompleteOnboarding();
        }

        await unitOfWork.SaveChangesAsync();

        return Result.Success;
    }
}