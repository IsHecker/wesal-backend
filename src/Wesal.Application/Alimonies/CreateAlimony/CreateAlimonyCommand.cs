using Wesal.Application.Messaging;

namespace Wesal.Application.Alimonies.CreateAlimony;

public record struct CreateAlimonyCommand(
    Guid CourtId,
    Guid StaffId,
    Guid CourtCaseId,
    long Amount,
    string Frequency,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;