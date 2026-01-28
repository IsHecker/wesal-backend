namespace Wesal.Contracts.Documents;

public record struct DocumentResponse(
    Guid Id,
    string FileName,
    long FileSizeBytes,
    string MimeType,
    string DownloadUrl,
    Guid UploadedBy,
    DateTime UploadedAt);