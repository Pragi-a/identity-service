using Identity.Application.Common.Errors;
using Identity.Application.Common.Errors.Helper;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Commands;
using MediatR;

namespace Identity.Application.Features.Users.DeactivateUser;

public sealed class DeactivateUserHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<DeactivateUserCommand, Result<DeactivateUserResponse>>
{
    public async Task<Result<DeactivateUserResponse>> Handle(DeactivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result<DeactivateUserResponse>.Failure(UserErrors.UserNotFound);

        await refreshTokenRepository.RevokeAllByUserIdAsync(request.UserId, DateTime.UtcNow, cancellationToken);

        user.DeActivate();

        return Result<DeactivateUserResponse>.Success(new DeactivateUserResponse());
    }
}