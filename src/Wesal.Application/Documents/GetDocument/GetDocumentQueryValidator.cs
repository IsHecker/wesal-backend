using FluentValidation;

namespace Wesal.Application.Documents.GetDocument;

public sealed class GetDocumentQueryValidator : AbstractValidator<GetDocumentQuery>
{
    public GetDocumentQueryValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("DocumentId is required");
    }
}