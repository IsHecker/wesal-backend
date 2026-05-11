using Wesal.Application.Messaging;

namespace Wesal.Application.Custodies.CreateCustody;

public record struct CreateCustodyCommand(
    Guid CourtId,
    Guid StaffId,
    Guid CourtCaseId,
    Guid CustodialParentId,
    DateTime StartAt,
    DateTime? EndAt) : ICommand<Guid>;