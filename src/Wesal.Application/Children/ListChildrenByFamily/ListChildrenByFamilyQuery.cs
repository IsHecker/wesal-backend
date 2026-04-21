using Wesal.Application.Messaging;
using Wesal.Contracts.Children;

namespace Wesal.Application.Children.ListChildrenByFamily;

public record struct ListChildrenByFamilyQuery(Guid FamilyId) : IQuery<IEnumerable<ChildResponse>>;