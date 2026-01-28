using Microsoft.AspNetCore.Http;
using Wesal.Application.Messaging;

namespace Wesal.Application.Documents.UploadDocument;

public record struct UploadDocumentCommand(Guid UploadedBy, IFormFile File)
    : ICommand<Guid>;