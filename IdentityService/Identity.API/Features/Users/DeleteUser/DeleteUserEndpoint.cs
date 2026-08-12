using Identity.API.Authorization;
using Identity.Application.Authorization;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Users.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Users.DeleteUser;

public static class DeleteUserEndpoint
{
    public static IEndpointRouteBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/users/{id}",
                async ([FromRoute] Guid userId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteUserCommand(userId);

                    var result = await sender.Send(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        if (result.Error == UserErrors.UserNotFound)
                        {
                            return Results.NotFound(
                                new
                                {
                                    Error = UserErrors.UserNotFound.Message,
                                });
                        }

                        return Results.BadRequest();
                    }

                    return Results.NoContent();
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Delete.Code))
            .WithTags("users")
            .WithName("DeleteUser")
            .WithSummary("Deletes a user")
            .WithDescription("Delete an user by user id")
            .Produces<DeleteUserResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
        return app;
    }
}