using Identity.Application.Common.Results;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    IReadOnlyCollection<Guid> RoleIds) 
    : IRequest<Result<CreateUserResponse>>;