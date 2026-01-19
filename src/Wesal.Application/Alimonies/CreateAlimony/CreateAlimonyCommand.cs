using Wesal.Application.Messaging;

namespace Wesal.Application.Alimonies.CreateAlimony;

public record struct CreateAlimonyCommand(
    Guid UserId,
    Guid CourtCaseId,
    Guid PayerId,
    Guid RecipientId,
    long Amount,
    string Frequency,
    int StartDayInMonth,
    int DueDay,
    DateTime StartAt,
    DateTime EndAt) : ICommand<Guid>;