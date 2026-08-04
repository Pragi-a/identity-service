using Identity.Application.Interfaces;
using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(IdentityDbContext context) : IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}