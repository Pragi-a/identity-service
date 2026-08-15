using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(IdentityDbContext context) : IRefreshTokenRepository
{
    public async Task AddRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshTokenHash,
        CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == refreshTokenHash,
                cancellationToken);
    }


    public async Task RevokeAllByUserIdAsync(Guid userId, DateTime revokedAt, CancellationToken cancellationToken)
    {
        await context.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ExecuteUpdateAsync(
                x => x.SetProperty(t => t.RevokedAt, revokedAt),
                cancellationToken);
    }

    public async Task<bool> TryConsumeAsync(Guid refreshTokenId, DateTime revokedAt,
        CancellationToken cancellationToken)
    {
        var affectedRows =
            await context.RefreshTokens
                .Where(x => x.Id == refreshTokenId && x.RevokedAt == null)
                .ExecuteUpdateAsync(x => x.SetProperty(x => revokedAt, revokedAt), cancellationToken);
        return affectedRows == 1;
    }
}