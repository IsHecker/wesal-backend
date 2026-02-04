using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Families;

namespace Wesal.Application.Families.ListFamiliesByCourt;

public sealed record ListFamiliesByCourtQuery(
    Guid CourtId,
    Pagination Pagination) : IQuery<PagedResponse<FamilyResponse>>;