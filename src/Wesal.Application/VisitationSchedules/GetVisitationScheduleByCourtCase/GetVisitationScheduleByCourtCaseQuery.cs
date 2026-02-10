using Wesal.Application.Messaging;
using Wesal.Contracts.VisitationSchedules;

namespace Wesal.Application.VisitationSchedules.GetVisitationScheduleByCourtCase;

public record struct GetVisitationScheduleByCourtCaseQuery(Guid CourtId, Guid CourtCaseId)
    : IQuery<VisitationScheduleResponse>;