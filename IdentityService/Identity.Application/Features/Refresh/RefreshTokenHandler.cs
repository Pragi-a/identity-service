using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Features.Login;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Refresh;

public class RefreshTokenHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher,
    ITokenProvider tokenProvider,
    IRefreshTokenGenerator refreshTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.refreshToken))
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);

        var refreshTokenHash = refreshTokenHasher.HashToken(request.refreshToken);

        var refreshToken = await refreshTokenRepository.GetByRefreshTokenAsync(refreshTokenHash, cancellationToken);

        if (refreshToken is null || !refreshToken.CanBeUsed(DateTime.UtcNow))
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);


        var user = await userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (user is null || !user.IsActive)
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);

        var accessToken = tokenProvider.AccessTokenResult(user);

        var newRefreshTokenStr = refreshTokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = refreshTokenHasher.HashToken(newRefreshTokenStr);

        refreshToken.Revoke(DateTime.UtcNow);
        
        var newRefreshToken = RefreshToken.Create(
            hashedRefreshToken,
            user.Id,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30)
        );

        await refreshTokenRepository.AddRefreshToken(newRefreshToken, cancellationToken);
        await  refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken, newRefreshTokenStr, string.Empty));
    }
}