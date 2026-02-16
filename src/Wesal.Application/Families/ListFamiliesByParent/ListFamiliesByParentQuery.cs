using Wesal.Application.Messaging;
using Wesal.Contracts.Families;

namespace Wesal.Application.Families.ListFamiliesByParent;

public record struct ListFamiliesByParentQuery(Guid ParentId) : IQuery<IEnumerable<FamilyResponse>>;