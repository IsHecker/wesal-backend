using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;

namespace Wesal.Application.Schools.GetSchoolProfile;

public sealed record GetSchoolProfileQuery(Guid SchoolId) : IQuery<SchoolResponse>;