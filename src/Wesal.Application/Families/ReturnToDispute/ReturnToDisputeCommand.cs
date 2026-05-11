using Wesal.Application.Messaging;

namespace Wesal.Application.Families.ReturnToDispute;

public record struct ReturnToDisputeCommand(
    Guid FamilyId,
    Guid SettlementStaffId) : ICommand;