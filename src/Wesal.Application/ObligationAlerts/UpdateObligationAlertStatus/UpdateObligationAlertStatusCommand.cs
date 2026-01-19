using Wesal.Application.Messaging;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

public record struct UpdateObligationAlertStatusCommand(
    Guid AlertId,
    Guid UserId,
    string Status,
    string? ResolutionNotes = null) : ICommand;