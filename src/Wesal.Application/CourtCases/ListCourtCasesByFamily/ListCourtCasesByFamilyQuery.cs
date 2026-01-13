using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtCases;

namespace Wesal.Application.CourtCases.ListCourtCasesByFamily;

public record struct ListCourtCasesByFamilyQuery(Guid FamilyId, Pagination Pagination)
    : IQuery<PagedResponse<CourtCaseResponse>>;