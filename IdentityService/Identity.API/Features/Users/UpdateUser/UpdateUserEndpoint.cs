using Identity.API.Authorization;
using Identity.Application.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Users.UpdateUser;

namespace Identity.API.Features.Users.UpdateUser;

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{id}",
                async ([AsParameters] Guid userId, [FromBody] UpdateUserRequest request, ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var command = new UpdateUserCommand(userId, request.Email, request.FirstName, request.LastName,
                        request.RoleIds);

                    var result = await sender.Send(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        if (result.Error == CommonErrors.ConcurrencyConflict)
                        {
                            return Results.Conflict(
                                new
                                {
                                    error = result.Error,
                                }
                            );
                        }

                        if (result.Error == UserErrors.UserNotFound)
                        {
                            return Results.NotFound(
                                new
                                {
                                    error = result.Error,
                                }
                            );
                        }

                        return Results.BadRequest();
                    }

                    return Results.Ok(result.Value);
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Update.Code))
            .WithTags("users")
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