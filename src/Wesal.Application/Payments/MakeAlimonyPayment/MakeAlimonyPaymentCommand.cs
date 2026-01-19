using Wesal.Application.Messaging;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

public record struct MakeAlimonyPaymentCommand(
    Guid UserId,
    Guid AlimonyId,
    Guid PaymetDueId,
    long Amount,
    string PaymentMethod) : ICommand<Guid>;