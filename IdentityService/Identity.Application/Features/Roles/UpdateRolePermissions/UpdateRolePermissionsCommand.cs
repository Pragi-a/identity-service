using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Roles.UpdateRolePermissions;

public sealed record UpdateRolePermissionsCommand(Guid RoleId, IEnumerable<Guid> PermissionIds)
    : IRequest<Result<UpdateRolePermissionsResponse>>;