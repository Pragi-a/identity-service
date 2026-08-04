using Identity.Application.Common.Errors;
using Identity.Application.Features.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Login;

public static class LoginEndpoint
{

    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async ([FromBody] LoginRequest request, ISender sender) =>
        {
            var command = new LoginCommand(request.Email, request.Password);
            
            var user = await sender.Send(command);

            if (user.IsFailure)
            {
                if (user.Error == UserErrors.InvalidCredentials)
                {
                    return Results.BadRequest(new
                    {
                        Error = UserErrors.InvalidCredentials.Message,
                    });
                }

                return Results.BadRequest();
            }

            return Results.Ok(user.Value);
        });

        return app;
    }
}