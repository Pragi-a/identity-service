using Identity.Application.Common.Errors;
using Identity.Application.Common.Errors.Helper;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using MediatR;

namespace Identity.Application.Features.Me;

public class GetCurrentUserHandler(IUserRepository userRepository)
    : IRequestHandler<GetCurrentUserQuery, Result<GetCurrentUserResponse>>
{
    public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null) return Result<GetCurrentUserResponse>.Failure(UserErrors.UserNotFound);

        return Result<GetCurrentUserResponse>.Success(new GetCurrentUserResponse(user.Id, user.Email.Value));
    }
}