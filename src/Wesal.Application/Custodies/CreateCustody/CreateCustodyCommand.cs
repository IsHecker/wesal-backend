using Wesal.Application.Messaging;

namespace Wesal.Application.Custodies.CreateCustody;

public record struct CreateCustodyCommand(
    Guid CourtId,
    Guid CourtCaseId,
    Guid CustodianId,
    DateTime StartAt,
    DateTime? EndAt) : ICommand<Guid>;