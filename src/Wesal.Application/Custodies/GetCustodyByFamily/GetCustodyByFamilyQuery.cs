using Wesal.Application.Messaging;
using Wesal.Contracts.Custodies;

namespace Wesal.Application.Custodies.GetCustodyByFamily;

public record struct GetCustodyByFamilyQuery(Guid FamilyId)
    : IQuery<CustodyResponse>;