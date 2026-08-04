using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Me;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Me;

public static class MeEndpoint
{
    public static IEndpointRouteBuilder MapMeEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me", async (ClaimsPrincipal user, [FromServices] ISender sender) =>
        {
            var userId = user.FindFirstValue(JwtRegisteredClaimNames.Sub)!;

            var result = await sender.Send(new GetCurrentUserQuery(Guid.Parse(userId)));

            if (result.IsFailure)
            {
                if (result.Error == UserErrors.UserNotFound)
                {
                    return Results.NotFound(new
                    {
                        Error = UserErrors.UserNotFound,
                    });
                }

                return Results.BadRequest();
            }

            return Results.Ok(result.Value);
        });

        return app;
    }
}