using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;

namespace Wesal.Application.Schools.GetSchool;

public sealed record GetSchoolQuery(Guid SchoolId) : IQuery<SchoolResponse>;