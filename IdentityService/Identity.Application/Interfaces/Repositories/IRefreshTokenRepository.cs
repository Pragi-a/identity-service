using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}