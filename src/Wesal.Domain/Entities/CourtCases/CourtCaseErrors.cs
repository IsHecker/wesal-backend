using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CourtCases;

public static class CourtCaseErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "CourtCase.NotFound",
            $"Court case with ID '{id}' was not found");
    public static Error CaseNumberAlreadyExists(string caseNumber) =>
        Error.Conflict("CourtCase.CaseNumberAlreadyExists", $"Case number '{caseNumber}' already exists");

    public static readonly Error FamilyHasOpenCase =
        Error.Validation(
            "CourtCase.FamilyHasOpenCase",
            "Cannot create a new case. This family already has an open court case. Close the existing case first.");

    public static readonly Error AlreadyClosed =
        Error.Validation(
            "CourtCase.AlreadyClosed",
            "This court case is already closed.");

    public static readonly Error CannotCloseWithPendingObligations =
        Error.Validation(
            "CourtCase.CannotCloseWithPendingObligations",
            "Cannot close case with pending obligations. Ensure all scheduled visitations are completed and all alimony payments are settled.");

    public static readonly Error Unauthorized =
        Error.Validation(
            "CourtCase.Unauthorized",
            "You are not authorized to perform this action on this court case.");
}