using Identity.Application.Interfaces.Repositories;

namespace Identity.Application.Features.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : ITransactionalRequest<DeactivateUserResponse>;