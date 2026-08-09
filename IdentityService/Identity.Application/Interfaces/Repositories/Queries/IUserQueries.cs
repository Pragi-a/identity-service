using Identity.Application.Features.Users.GetUsers.Projections;

namespace Identity.Application.Interfaces.Repositories.Queries;

public interface IUserQueries
{
    Task<UserListResult> GetAllUsers(int pageSize, int currentPage,
        CancellationToken cancellationToken);
}