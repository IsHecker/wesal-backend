using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Complaints;

public static class ComplaintErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Complaint.NotFound", $"Complaint with ID '{id}' was not found");

    public static Error ComplaintMismatch =>
        Error.Forbidden(
            "Complaint.ComplaintMismatch",
            "This complaint is not related to your court.");

    public static Error MaxCountExceeded(int maxCount) =>
        Error.Validation(
            "Complaint.MaxCountExceeded",
            $"You've exceeded your monthly complaint limit of {maxCount}.");

    public static Error CannotUpdateStatus(ComplaintStatus status) =>
        Error.Validation(
            "Complaint.CannotUpdateStatus",
            $"Cannot update complaint status to '{status}'. Only 'UnderReview' or 'Resolved' transitions are supported.");
}