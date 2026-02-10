using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Results;

namespace Wesal.Application.Documents.UploadDocument;

internal sealed class UploadDocumentCommandHandler(
    ICloudinaryService cloudinaryService,
    IRepository<Document> documentRepository)
    : ICommandHandler<UploadDocumentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        UploadDocumentCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null)
            return DocumentErrors.FileRequired;

        var uploadResult = await cloudinaryService.UploadFileAsync(
            request.File,
            cancellationToken);

        if (uploadResult.IsFailure)
            return DocumentErrors.UploadFailed;

        var document = Document.Create(
            request.UserId,
            request.File.FileName,
            uploadResult.Value.Bytes,
            request.File.ContentType,
            uploadResult.Value.PublicId,
            uploadResult.Value.Url);

        await documentRepository.AddAsync(document, cancellationToken);

        return document.Id;
    }
}