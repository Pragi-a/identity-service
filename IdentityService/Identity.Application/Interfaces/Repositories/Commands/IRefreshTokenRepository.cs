using Identity.Application.Common.Results;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories.Commands;

public interface IRefreshTokenRepository
{
    Task AddRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken);
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken);
    Task RevokeAllByUserIdAsync(Guid userId, DateTime revokedAt, CancellationToken cancellationToken);
}