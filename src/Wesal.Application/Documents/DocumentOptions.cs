namespace Wesal.Application.Documents;

public sealed class DocumentOptions
{
    public const string SectionName = "Document";

    public long MaxFileSizeBytes { get; init; }

    public string[] AllowedMimeTypes { get; init; } = null!;
}