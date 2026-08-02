using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Register;

public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : 
    IRequest<Result<RegisterUserResponse>>;
