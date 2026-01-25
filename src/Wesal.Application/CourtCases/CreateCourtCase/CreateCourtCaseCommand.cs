using Wesal.Application.Messaging;

namespace Wesal.Application.CourtCases.CreateCourtCase;

public record struct CreateCourtCaseCommand(
    Guid UserId,
    Guid FamilyId,
    string CaseNumber,
    string DecisionSummary) : ICommand<Guid>;