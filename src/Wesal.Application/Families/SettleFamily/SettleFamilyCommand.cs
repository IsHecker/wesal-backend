using Wesal.Application.Messaging;

namespace Wesal.Application.Families.SettleFamily;

public record struct SettleFamilyCommand(
    Guid FamilyId,
    Guid SettlementStaffId) : ICommand;