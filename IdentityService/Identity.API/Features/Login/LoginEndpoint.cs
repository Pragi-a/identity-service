using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Identity.Application.Features.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async ([FromBody] LoginRequest request, ISender sender) =>
            {
                var command = new LoginCommand(request.Email, request.Password);

                var result = await sender.Send(command);

                return ApiResult<LoginResponse>.Create(result, x => SuccessResponse.Ok());
            })
            .WithName("Login user")
            .WithTags("Users")
            .WithSummary("Logs in the user")
            .WithDescription("Used to login an user")
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithOpenApi();

        return app;
    }
}