using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Results;

namespace Wesal.Application.Documents.DeleteDocument;

internal sealed class DeleteDocumentCommandHandler(
    ICloudinaryService cloudinaryService,
    IRepository<Document> documentRepository)
    : ICommandHandler<DeleteDocumentCommand>
{
    public async Task<Result> Handle(
        DeleteDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(
            request.DocumentId,
            cancellationToken);

        if (document is null)
            return DocumentErrors.NotFound(request.DocumentId);

        if (document.UploadedBy != request.UserId)
            return DocumentErrors.CannotSeeOrModifyDocument;

        var deleteResult = await cloudinaryService.DeleteFileAsync(
            document.CloudinaryPublicId,
            cancellationToken);

        if (deleteResult.IsFailure)
            return DocumentErrors.DeleteFailed;

        documentRepository.Delete(document);

        return Result.Success;
    }
}