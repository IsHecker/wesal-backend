using Wesal.Application.Messaging;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

public record struct MakeAlimonyPaymentCommand(
    Guid ParentId,
    Guid PaymetDueId,
    string PaymentMethod) : ICommand<Guid>;