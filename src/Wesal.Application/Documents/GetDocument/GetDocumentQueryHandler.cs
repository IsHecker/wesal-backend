using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Documents;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Results;

namespace Wesal.Application.Documents.GetDocument;

internal sealed class GetDocumentQueryHandler(
    IRepository<Document> documentRepository)
    : IQueryHandler<GetDocumentQuery, DocumentResponse>
{
    public async Task<Result<DocumentResponse>> Handle(
        GetDocumentQuery request,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(
            request.DocumentId,
            cancellationToken);

        if (document is null)
            return DocumentErrors.NotFound(request.DocumentId);

        return new DocumentResponse(
            document.Id,
            document.FileName,
            document.FileSizeBytes,
            document.MimeType,
            document.CloudinaryUrl,
            document.UploadedBy,
            document.UploadedAt);
    }
}