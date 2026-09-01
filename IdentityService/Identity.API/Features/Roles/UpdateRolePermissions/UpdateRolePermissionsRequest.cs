namespace Identity.API.Features.Roles.UpdateRolePermissions;

public sealed record UpdateRolePermissionsRequest(IEnumerable<Guid> RoleIds);