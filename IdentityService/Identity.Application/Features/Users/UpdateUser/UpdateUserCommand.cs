using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyCollection<Guid> RoleIds) : IRequest<Result<UpdateUserResponse>>;