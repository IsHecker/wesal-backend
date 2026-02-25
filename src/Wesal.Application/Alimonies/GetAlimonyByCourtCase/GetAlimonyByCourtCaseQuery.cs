using Wesal.Application.Messaging;
using Wesal.Contracts.Alimonies;

namespace Wesal.Application.Alimonies.GetAlimonyByCourtCase;

public record struct GetAlimonyByCourtCaseQuery(Guid UserId, Guid CourtId, Guid CourtCaseId, bool IsParent)
    : IQuery<AlimonyResponse>;