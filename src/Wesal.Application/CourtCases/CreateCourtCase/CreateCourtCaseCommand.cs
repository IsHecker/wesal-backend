using Wesal.Application.Messaging;

namespace Wesal.Application.CourtCases.CreateCourtCase;

public record struct CreateCourtCaseCommand(
    Guid CourtId,
    Guid FamilyId,
    string CaseNumber,
    DateTime FiledAt,
    string Status,
    string DecisionSummary) : ICommand<Guid>;