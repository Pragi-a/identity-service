using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Identity.Application.Features.Roles.UpdateRolePermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Roles.UpdateRolePermissions;

public static class UpdateRolePermissionEndpoint
{
    public static IEndpointRouteBuilder MapUpdateRolePermissions(this IEndpointRouteBuilder app)
    {
        app.MapPut("/roles/{roleId}",
                async ([FromRoute] Guid roleId, [FromBody] UpdateRolePermissionsRequest request, ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var command = new UpdateRolePermissionsCommand(roleId, request.RoleIds);
                    var result = await sender.Send(command, cancellationToken);
                    return ApiResult<UpdateRolePermissionsResponse>.Create(result, _ => SuccessResponse.Ok());
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Roles.Update.Code))
            .WithTags("Roles")
            .WithName("UpdateRolePermissions")
            .WithSummary("Update Permission")
            .WithDescription("Update the Role with the new set of Permissions")
            .Produces<UpdateRolePermissionsResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}