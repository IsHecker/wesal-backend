using Wesal.Application.Messaging;

namespace Wesal.Application.Documents.DeleteDocument;

public record struct DeleteDocumentCommand(Guid UserId, Guid DocumentId) : ICommand;