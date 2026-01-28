using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Documents;

public sealed class Document : Entity
{
    public Guid UploadedBy { get; private set; }

    public string FileName { get; private set; } = null!;
    public long FileSizeBytes { get; private set; }
    public string MimeType { get; private set; } = null!;
    public string CloudinaryPublicId { get; private set; } = null!;
    public string CloudinaryUrl { get; private set; } = null!;
    public DateTime UploadedAt { get; private set; }

    private Document() { }

    public static Document Create(
        Guid uploadedBy,
        string fileName,
        long fileSizeBytes,
        string mimeType,
        string cloudinaryPublicId,
        string cloudinaryUrl)
    {
        return new Document
        {
            UploadedBy = uploadedBy,
            FileName = fileName,
            FileSizeBytes = fileSizeBytes,
            MimeType = mimeType,
            CloudinaryPublicId = cloudinaryPublicId,
            CloudinaryUrl = cloudinaryUrl,
            UploadedAt = DateTime.UtcNow
        };
    }
}