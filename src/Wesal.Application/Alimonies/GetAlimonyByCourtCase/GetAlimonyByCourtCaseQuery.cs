using Wesal.Application.Messaging;
using Wesal.Contracts.Alimonies;

namespace Wesal.Application.Alimonies.GetAlimonyByCourtCase;

public record struct GetAlimonyByCourtCaseQuery(Guid CourtId, Guid CourtCaseId)
    : IQuery<AlimonyResponse>;