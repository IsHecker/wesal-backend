using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Documents;

public static class DocumentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Documents.NotFound",
            $"Document with ID '{id}' was not found");

    public static readonly Error UploadFailed =
        Error.Failure(
            "Documents.UploadFailed",
            "Failed to upload document to external storage");

    public static readonly Error DeleteFailed =
        Error.Failure(
            "Documents.DeleteFailed",
            "Failed to delete document from external storage");

    public static readonly Error CannotSeeOrModifyDocument =
        Error.Forbidden(
            "Documents.CannotModifyDocument",
            "You're not allowed to see or modify this document.");

    public static readonly Error FileRequired =
        Error.Validation(
            "Documents.FileRequired",
            "File is required");
}