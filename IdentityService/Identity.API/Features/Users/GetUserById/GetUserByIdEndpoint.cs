using MediatR;
using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Identity.Application.Features.Users.GetUserById;

namespace Identity.API.Features.Users.GetUserById;

public static class GetUserByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}",
                async ([AsParameters] GetUserByIdRequest request, ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var query = new GetUserByIdQuery(request.UserId);

                    var result = await sender.Send(query, cancellationToken);

                    return ApiResult<GetUserByIdResponse>.Create(result, _ => SuccessResponse.Ok());
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