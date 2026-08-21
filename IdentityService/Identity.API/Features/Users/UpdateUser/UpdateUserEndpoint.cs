using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Features.Users.UpdateUser;

namespace Identity.API.Features.Users.UpdateUser;

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{userId}",
                async ([FromRoute] Guid userId, [FromBody] UpdateUserRequest request, ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var command = new UpdateUserCommand(userId, request.Email, request.FirstName, request.LastName,
                        request.RoleIds);

                    var result = await sender.Send(command, cancellationToken);

                    return ApiResult<UpdateUserResponse>.Create(result, _ => SuccessResponse.Ok());
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Update.Code))
            .WithTags("Users")
            .WithName("UpdateUser")
            .WithSummary("User Update")
            .WithDescription("Updates an existing user")
            .Produces<UpdateUserResponse>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);
        return app;
    }
}