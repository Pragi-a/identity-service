using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : IRequest<Result<DeleteUserResponse>>;