using Wesal.Application.Messaging;
using Wesal.Contracts.PaymentGateway;

namespace Wesal.Application.PaymentsDue.InitiateAlimonyPayment;

public record struct InitiateAlimonyPaymentCommand(
    Guid ParentId,
    Guid PaymentDueId,
    string SuccessUrl,
    string CancelUrl) : ICommand<SessionResponse>;