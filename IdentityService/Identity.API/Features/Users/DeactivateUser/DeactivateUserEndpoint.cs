using MediatR;
using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Identity.Application.Features.Users.DeactivateUser;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Users.DeactivateUser;

public static class DeactivateUserEndpoint
{
    public static IEndpointRouteBuilder MapDeactivateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete(("/api/users/{id}/deactivate"), async (
                [FromRoute] Guid id, ISender sender, CancellationToken cancellationToken
            ) =>
            {
                var command = new DeactivateUserCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return ApiResult<DeactivateUserResponse>.Create(result, _ => SuccessResponse.NoContent());
            })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Delete.Code))
            .WithName("DeactivateUser")
            .WithTags("users")
            .WithSummary("Deactivate an User")
            .WithDescription("Soft Deletes an Users")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi();


        return app;
    }
}