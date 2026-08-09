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
            .Select(x => new UserListItem(x.Id, x.Email.Value, x.FirstName, x.LastName))
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new UserListResult(userList, count);
    }
}