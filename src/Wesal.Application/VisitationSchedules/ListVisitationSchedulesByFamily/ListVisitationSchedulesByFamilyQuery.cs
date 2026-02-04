using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationSchedules;

namespace Wesal.Application.VisitationSchedules.ListVisitationSchedulesByFamily;

public record struct ListVisitationSchedulesByFamilyQuery(
    Guid CourtId,
    Guid FamilyId,
    Pagination Pagination) : IQuery<PagedResponse<VisitationScheduleResponse>>;