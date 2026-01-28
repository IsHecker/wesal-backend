using Wesal.Application.Messaging;
using Wesal.Contracts.Documents;

namespace Wesal.Application.Documents.GetDocument;

public record struct GetDocumentQuery(Guid DocumentId) : IQuery<DocumentResponse>;