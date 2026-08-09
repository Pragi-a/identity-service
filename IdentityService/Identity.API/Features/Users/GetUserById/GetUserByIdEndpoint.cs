using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.API.Authorization;
using Identity.Application.Authorization;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Users.GetUserById;

namespace Identity.API.Features.Users.GetUserById;

public static class GetUserByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id}",
                async ([FromRoute] GetUserByIdRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetUserByIdQuery(request.UserId);

                    var result = await sender.Send(query, cancellationToken);

                    if (result.IsFailure)
                    {
                        if (result.Error == UserErrors.UserNotFound)
                        {
                            return Results.NotFound(new
                            {
                                error = result.Error
                            });
                        }

                        return Results.BadRequest(new
                        {
                            error = result.Error
                        });
                    }

                    return Results.Ok(result.Value);
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.View.Code))
            .WithName("GetUserById")
            .WithTags("Users")
            .WithSummary("Get User by Id")
            .WithDescription("Endpoint to fetch user details")
            .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
        return app;
    }
}