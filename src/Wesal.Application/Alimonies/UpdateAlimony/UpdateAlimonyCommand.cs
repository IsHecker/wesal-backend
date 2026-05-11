using Wesal.Application.Messaging;

namespace Wesal.Application.Alimonies.UpdateAlimony;

public record struct UpdateAlimonyCommand(
    Guid CourtId,
    Guid StaffId,
    Guid AlimonyId,
    long Amount,
    string Frequency,
    DateOnly StartDate,
    DateOnly? EndDate) : ICommand;