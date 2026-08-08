using Identity.API.Authorization;
using Identity.Application.Authorization;
using MediatR;
using Identity.Application.Common.Errors;
using Identity.Application.Features.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Features.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users",
                async ([FromBody] CreateUserRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var createUserCommand = new CreateUserCommand(
                        request.Email,
                        request.Password,
                        request.FirstName,
                        request.LastName,
                        request.RoleIds
                    );

                    var result = await sender.Send(createUserCommand, cancellationToken);

                    if (result.IsFailure)
                    {
                        if (result.Error == UserErrors.EmailAlreadyExists)
                        {
                            return Results.BadRequest(new
                            {
                                Error = UserErrors.EmailAlreadyExists.Message
                            });
                        }

                        if (result.Error == UserErrors.InvalidRole)
                        {
                            return Results.BadRequest(new
                            {
                                Error = UserErrors.InvalidRole.Message
                            });
                        }

                        return Results.BadRequest();
                    }

                    return  Results.Created(
                        $"/api/users/{result.Value.Id}",
                        result.Value);
                })
            .WithMetadata(new HasPermissionAttribute(PermissionCodes.Users.Create.Code))
            .WithName("CreateUser")
            .WithTags("Users")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user and assigns one or more roles.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithOpenApi();
        
        return app;
    }
}