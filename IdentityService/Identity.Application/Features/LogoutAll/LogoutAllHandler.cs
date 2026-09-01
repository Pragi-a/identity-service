using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using MediatR;

namespace Identity.Application.Features.LogoutAll;

public class LogoutAllHandler(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenHasher refreshTokenHasher)
    : IRequestHandler<LogoutAllCommand, Result<LogoutAllResponse>>
{
    public async Task<Result<LogoutAllResponse>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.refreshToken))
            return Result<LogoutAllResponse>.Success(new LogoutAllResponse());

        var refreshTokenHash = refreshTokenHasher.HashToken(request.refreshToken);
        var refreshToken = await refreshTokenRepository.GetByRefreshTokenAsync(refreshTokenHash, cancellationToken);

        if (refreshToken is null)
            return Result<LogoutAllResponse>.Success(new LogoutAllResponse());

        await refreshTokenRepository.RevokeAllByUserIdAsync(refreshToken.UserId, DateTime.UtcNow, cancellationToken);
        return Result<LogoutAllResponse>.Success(new LogoutAllResponse());
    }
}