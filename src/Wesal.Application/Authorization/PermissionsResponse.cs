namespace Wesal.Application.Authorization;

public record struct PermissionsResponse(Guid UserId, HashSet<string> Permissions);