using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Features.Login;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Repositories.Queries;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Refresh;

public class RefreshTokenHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher,
    ITokenProvider tokenProvider,
    IRefreshTokenGenerator refreshTokenGenerator,
    IUserAuthorizationQueries userAuthorizationQueries)
    : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);

        var refreshTokenHash = refreshTokenHasher.HashToken(request.RefreshToken);

        var refreshToken = await refreshTokenRepository.GetByRefreshTokenAsync(refreshTokenHash, cancellationToken);

        if (refreshToken is null || !refreshToken.CanBeUsed(DateTime.UtcNow))
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);


        var user = await userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (user is null || !user.IsActive)
            return Result<LoginResponse>.Failure(UserErrors.InvalidRefreshToken);

        
        /*
         When 2 requests comes in for refreshing a token, 2 refresh tokens would branch out from the 1st token because of concurrency
         In order to avoid that we use direct set-based update here. 
         This is in violation of the DDD rule of hydrating and then updating the aggregate. 
         Should modify this later.
        
        */
        var refreshed = await  refreshTokenRepository.TryConsumeAsync(refreshToken.Id, DateTime.UtcNow, cancellationToken);

        if (!refreshed)
        {
            return Result<LoginResponse>.Failure(UserErrors.AlreadyUsedRefreshToken);
        }
        
        var permissionCodes = await userAuthorizationQueries.GetPermissionCodesAsync(user.Id, cancellationToken);
        var accessToken = tokenProvider.GenerateAccessToken(user, permissionCodes);

        var newRefreshTokenStr = refreshTokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = refreshTokenHasher.HashToken(newRefreshTokenStr);

        var newRefreshToken = RefreshToken.Create(
            hashedRefreshToken,
            user.Id,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(30)
        );

        await refreshTokenRepository.AddRefreshToken(newRefreshToken, cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken, newRefreshTokenStr, string.Empty));
    }
}