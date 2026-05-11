using Wesal.Application.Messaging;

namespace Wesal.Application.CourtCases.EscalateToCourtCase;

public record struct EscalateToCourtCaseCommand(
    Guid CourtId,
    Guid SettlementStaffId,
    Guid FamilyId,
    Guid? DocumentId,
    string CaseNumber,
    string DecisionSummary) : ICommand<Guid>;