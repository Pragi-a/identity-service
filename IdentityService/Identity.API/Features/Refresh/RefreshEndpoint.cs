using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Identity.Application.Features.Login;
using Identity.Application.Features.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Refresh;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefreshEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", async ([FromBody] RefreshRequest request, ISender sender) =>
            {
                var command = new RefreshTokenCommand(request.refreshToken);

                var result = await sender.Send(command);

                return ApiResult<LoginResponse>.Create(result, _ => SuccessResponse.Ok());
            })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Update.Code))
            .WithName("Update refresh token")
            .WithTags("Users")
            .WithSummary("Updates the refresh token")
            .WithDescription("Update the refresh token")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithOpenApi();

        return app;
    }
}