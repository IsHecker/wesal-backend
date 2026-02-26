using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Children;
using Wesal.Contracts.Common;

namespace Wesal.Application.Children.ListChildrenBySchool;

public record struct ListChildrenBySchoolQuery(
    Guid SchoolId,
    string? Name,
    Pagination Pagination) : IQuery<PagedResponse<ChildResponse>>;