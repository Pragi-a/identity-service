using Identity.Application.Common.Errors;
using Identity.Application.Common.Errors.Helper;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Queries;
using MediatR;

namespace Identity.Application.Features.Users.GetUserById;

public sealed class GetUserByIdHandler(IUserQueries userQueries)
    : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdResponse>>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetUserById(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<GetUserByIdResponse>.Failure(UserErrors.UserNotFound);
        }

        return Result<GetUserByIdResponse>.Success(new GetUserByIdResponse(user.Email, user.FirstName, user.LastName));
    }
}