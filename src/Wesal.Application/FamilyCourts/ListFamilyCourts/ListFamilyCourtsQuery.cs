using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.FamilyCourts;

namespace Wesal.Application.FamilyCourts.ListFamilyCourts;

public sealed record ListFamilyCourtsQuery(
    string? Name,
    string? Governorate,
    Pagination Pagination) : IQuery<PagedResponse<FamilyCourtResponse>>;