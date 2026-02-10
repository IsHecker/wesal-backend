using Wesal.Application.Messaging;

namespace Wesal.Application.PaymentsDue.WithdrawPayment;

public record struct WithdrawPaymentCommand(
    Guid ParentId,
    Guid PaymentDueId,
    string WithdrawalMethod) : ICommand;