
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
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(
                x => x.SetProperty(t => t.RevokedAt, revokedAt),
                cancellationToken);
    }
}