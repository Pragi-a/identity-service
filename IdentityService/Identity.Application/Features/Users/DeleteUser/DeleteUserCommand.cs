using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories;
using MediatR;

namespace Identity.Application.Features.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : ITransactionalRequest<DeleteUserResponse>;