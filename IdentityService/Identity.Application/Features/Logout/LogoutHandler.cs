using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using MediatR;

namespace Identity.Application.Features.Logout;

public class LogoutHandler(IRefreshTokenRepository refreshTokenRepository,IRefreshTokenHasher refreshTokenHasher) 
    : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    public async Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.refreshToken))
            return Result<LogoutResponse>.Success(new LogoutResponse());
        
        var refreshTokenHash = refreshTokenHasher.HashToken(request.refreshToken);

        var refreshToken = await refreshTokenRepository.GetByRefreshTokenAsync(refreshTokenHash, cancellationToken);
        
        if (refreshToken is null)
            return Result<LogoutResponse>.Success(new LogoutResponse());
        
        refreshToken.Revoke(DateTime.UtcNow);
        
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        
        return Result<LogoutResponse>.Success(new LogoutResponse());
    }
}