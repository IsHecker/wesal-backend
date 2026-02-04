using Wesal.Application.Messaging;

namespace Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;

public record struct UpdateObligationAlertStatusCommand(
    Guid CourtId,
    Guid AlertId,
    string Status,
    string? ResolutionNotes = null) : ICommand;