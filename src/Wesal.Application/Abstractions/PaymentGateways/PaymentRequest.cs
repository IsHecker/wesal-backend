namespace Wesal.Application.Abstractions.PaymentGateways;

public abstract record PaymentRequest(long Amount, string Currency);