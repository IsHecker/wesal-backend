using Wesal.Application.Messaging;
using Wesal.Contracts.Documents;

namespace Wesal.Application.Documents.GetDocument;

public record struct GetDocumentQuery(Guid UserId, Guid DocumentId) : IQuery<DocumentResponse>;