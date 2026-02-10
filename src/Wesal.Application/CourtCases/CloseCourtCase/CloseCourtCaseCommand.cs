using Wesal.Application.Messaging;

namespace Wesal.Application.CourtCases.CloseCourtCase;

public record struct CloseCourtCaseCommand(
    Guid CourtId,
    Guid CourtCaseId,
    string ClosureNotes) : ICommand;