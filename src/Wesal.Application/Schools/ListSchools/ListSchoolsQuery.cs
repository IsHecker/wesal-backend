using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Schools;

namespace Wesal.Application.Schools.ListSchools;

public sealed record ListSchoolsQuery(
    Guid StaffId,
    string? Name,
    Pagination Pagination) : IQuery<PagedResponse<SchoolResponse>>;