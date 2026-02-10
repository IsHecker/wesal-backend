using Wesal.Application.Messaging;
using Wesal.Contracts.PaymentGateway;

namespace Wesal.Application.PaymentProcessing.CreateOnboardingSession;

public readonly record struct CreateOnboardingSessionCommand(
    Guid ParentId,
    string RefreshUrl,
    string ReturnUrl) : ICommand<SessionResponse>;