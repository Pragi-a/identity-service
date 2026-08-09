using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Queries;
using MediatR;

namespace Identity.Application.Features.Users.GetUsers;

public sealed class GetUsersHandler(IUserQueries userQueries) : IRequestHandler<GetUsersQuery, Result<GetUsersResponse>>
{
    public async Task<Result<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var userListResult = await userQueries.GetAllUsers(request.PageSize, request.CurrentPage, cancellationToken);

        return Result<GetUsersResponse>.Success(new GetUsersResponse(userListResult.UserListItems, request.PageSize,
            request.CurrentPage,
            userListResult.TotalCount));
    }
}