using FluentValidation;

namespace Wesal.Application.Documents.DeleteDocument;

public sealed class DeleteDocumentCommandValidator : AbstractValidator<DeleteDocumentCommand>
{
    public DeleteDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("DocumentId is required");
    }
}