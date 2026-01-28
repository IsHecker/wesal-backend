using FluentValidation;

namespace Wesal.Application.Documents.UploadDocument;

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    // TODO: Move it to configuration files.
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB

    private static readonly string[] AllowedMimeTypes =
    [
        "application/pdf",
        "application/msword",
        "image/jpeg",
        "image/png"
    ];

    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(MaxFileSizeBytes)
            .When(x => x.File is not null)
            .WithMessage($"File size must not exceed {MaxFileSizeBytes / 1024 / 1024}MB");

        RuleFor(x => x.File.ContentType)
            .Must(contentType => AllowedMimeTypes.Contains(contentType))
            .When(x => x.File is not null)
            .WithMessage("File type is not allowed. Accepted types: PDF, DOC, DOCX, JPG, PNG");
    }
}