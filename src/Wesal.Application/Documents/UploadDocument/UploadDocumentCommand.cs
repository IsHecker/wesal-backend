using Microsoft.AspNetCore.Http;
using Wesal.Application.Messaging;

namespace Wesal.Application.Documents.UploadDocument;

public record struct UploadDocumentCommand(Guid UserId, IFormFile File)
    : ICommand<Guid>;