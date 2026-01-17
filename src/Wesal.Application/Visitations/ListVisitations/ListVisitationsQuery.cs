using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;

namespace Wesal.Application.Visitations.ListVisitations;

public record struct ListVisitationsQuery(
    Guid? FamilyId,
    string? NationalId,
    string? Status,
    DateOnly? Date,
    Pagination Pagination) : IQuery<PagedResponse<VisitationResponse>>;