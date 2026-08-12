using Identity.API.Authorization;
using Identity.Application.Authorization;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Users.DeactivateUser;
using MediatR;
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

                if (result.IsFailure)
                {
                    if (result.Error == UserErrors.UserNotFound)
                    {
                        return Results.NotFound(new
                            {
                                Error = UserErrors.UserNotFound.Message,
                            }
                        );
                    }

                    if (result.Error == CommonErrors.ConcurrencyConflict)
                    {
                        return Results.Conflict(
                            new
                            {
                                error = CommonErrors.ConcurrencyConflict.Message
                            }
                        );
                    }

                    return Results.BadRequest(new { error = result.Error });
                }

                return Results.NoContent();
            })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Delete.Code))
            .WithName("DeactivateUser")
            .WithTags("users")
            .WithSummary("Deactivate an User")
            .WithDescription("Soft Deletes an Users")
            .Produces<DeactivateUserResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi();


        return app;
    }
}