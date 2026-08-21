using Identity.API.Authorization;
using Identity.API.Common.Results;
using Identity.Application.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Features.Register;
using MediatR;

namespace Identity.API.Features.Register;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async ([FromBody] RegisterRequest request, ISender sender) =>
            {
                var command = new RegisterUserCommand(request.Email, request.Password, request.FirstName,
                    request.LastName);

                var result = await sender.Send(command);

                return ApiResult<RegisterUserResponse>.Create(result, _ => SuccessResponse.Ok());
            }).WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Create.Code))
            .WithName("RegisterUser")
            .WithTags("Users")
            .WithSummary("Register a new user")
            .WithDescription("Resgisters a new user")
            .Produces<RegisterUserResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithOpenApi();


        return app;
    }
}