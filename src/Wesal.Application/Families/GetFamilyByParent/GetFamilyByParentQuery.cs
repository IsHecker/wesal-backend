using Wesal.Application.Messaging;
using Wesal.Contracts.Families;

namespace Wesal.Application.Families.GetFamilyByParent;

public record struct GetFamilyByParentQuery(Guid ParentId) : IQuery<FamilyResponse>;