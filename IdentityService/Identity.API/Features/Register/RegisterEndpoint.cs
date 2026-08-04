using Identity.Application.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Common.Results;
using Identity.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.API.Features.Register;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/register", async ([FromBody] RegisterRequest request, ISender sender) =>
        {
            var command = new RegisterUserCommand(request.Email, request.Password, request.FirstName, request.LastName);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                if (result.Error == UserErrors.EmailAlreadyExists)
                {
                    return Results.Conflict(new
                    {
                        error = result.Error.Message
                    });
                }

                return Results.BadRequest();
            }


            return Results.Created(string.Empty, result.Value);
        });

        return app;
    }
}