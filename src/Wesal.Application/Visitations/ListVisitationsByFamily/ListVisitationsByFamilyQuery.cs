using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;

namespace Wesal.Application.Visitations.ListVisitationsByFamily;

public record struct ListVisitationsByFamilyQuery(Guid FamilyId, Pagination Pagination)
    : IQuery<PagedResponse<VisitationResponse>>;