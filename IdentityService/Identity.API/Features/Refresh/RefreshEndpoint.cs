using Identity.Application.Common.Errors;
using Identity.Application.Features.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Refresh;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefreshEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async ([FromBody] RefreshRequest request, ISender sender) =>
        {
            var command = new RefreshTokenCommand(request.refreshToken);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                if (result.Error == UserErrors.InvalidRefreshToken)
                    return Results.NotFound(new
                    {
                        Error = result.Error.Message,
                    });

                return Results.BadRequest();
            }
            
            return  Results.Ok(result);
        });

        return app;
    }
}