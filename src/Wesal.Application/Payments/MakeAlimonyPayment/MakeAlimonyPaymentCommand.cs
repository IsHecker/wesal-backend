using Wesal.Application.Messaging;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

public record struct MakeAlimonyPaymentCommand(
    Guid UserId,
    Guid PaymetDueId,
    string PaymentMethod) : ICommand<Guid>;