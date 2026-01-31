using Wesal.Application.Messaging;

namespace Wesal.Application.CourtCases.CreateCourtCase;

public record struct CreateCourtCaseCommand(
    Guid StaffId,
    Guid FamilyId,
    Guid? DocumentId,
    string CaseNumber,
    string DecisionSummary) : ICommand<Guid>;