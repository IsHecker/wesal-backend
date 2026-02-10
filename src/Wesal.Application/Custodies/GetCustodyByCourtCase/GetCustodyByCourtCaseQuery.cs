using Wesal.Application.Messaging;
using Wesal.Contracts.Custodies;

namespace Wesal.Application.Custodies.GetCustodyByCourtCase;

public record struct GetCustodyByCourtCaseQuery(Guid CourtCaseId)
    : IQuery<CustodyResponse>;