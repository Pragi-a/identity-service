using Identity.API.Authorization;
using Identity.Application.Authorization;
using Identity.Application.Features.Users.GetUsers;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.API.Features.Users.GetUsers;

public static class GetUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetUsersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users",
                async ([AsParameters] GetUsersRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetUsersQuery(request.PageSize, request.Page);

                    var result = await sender.Send(query, cancellationToken);

                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                })
            .WithMetadata(
                new HasPermissionAttribute(PermissionCodes.Users.View.Code)
            )
            .WithName("GetUsers")
            .WithTags("Users")
            .WithSummary("Fetch All Users")
            .WithDescription("This endpoint is used to fetch all the users in the system.")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        return app;
    }
}