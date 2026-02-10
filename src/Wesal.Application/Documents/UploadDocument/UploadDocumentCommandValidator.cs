using FluentValidation;
using Microsoft.Extensions.Options;

namespace Wesal.Application.Documents.UploadDocument;

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator(IOptions<DocumentOptions> options)
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(options.Value.MaxFileSizeBytes)
            .When(x => x.File is not null)
            .WithMessage($"File size must not exceed {options.Value.MaxFileSizeBytes / 1024 / 1024}MB");

        RuleFor(x => x.File.ContentType)
            .Must(contentType => options.Value.AllowedMimeTypes.Contains(contentType))
            .When(x => x.File is not null)
            .WithMessage("File type is not allowed. Accepted types: PDF, DOC, DOCX, JPG, PNG");
    }
}