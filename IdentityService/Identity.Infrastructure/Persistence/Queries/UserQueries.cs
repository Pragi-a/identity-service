using Identity.Application.Features.Users.GetUserById.Projections;
using Identity.Application.Features.Users.GetUsers.Projections;
using Identity.Application.Interfaces.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Queries;

public sealed class UserQueries(IdentityDbContext context) : IUserQueries
{
    public async Task<UserListResult> GetAllUsers(int pageSize, int currentPage,
        CancellationToken cancellationToken)
    {
        var count = await context.Users
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var userList = await context.Users
            .AsNoTracking()
            .Select(x => new UserListItem(x.Id, x.Email.Value, x.FirstName.Value, x.LastName.Value))
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new UserListResult(userList, count);
    }

    public async Task<UserItem?> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, Item = new UserItem(x.Email.Value, x.FirstName.Value, x.LastName.Value) })
            .Where(x => x.Id == userId)
            .Select(x => x.Item)
            .SingleOrDefaultAsync(cancellationToken);
    }
}