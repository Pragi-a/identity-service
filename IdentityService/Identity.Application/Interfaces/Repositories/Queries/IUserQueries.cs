using Identity.Application.Features.Users.GetUserById.Projections;
using Identity.Application.Features.Users.GetUsers.Projections;

namespace Identity.Application.Interfaces.Repositories.Queries;

public interface IUserQueries
{
    Task<UserListResult> GetAllUsers(int pageSize, int currentPage,
        CancellationToken cancellationToken);
    
    Task<UserItem?> GetUserById(Guid userId, CancellationToken cancellationToken);
    
}