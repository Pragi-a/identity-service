using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Commands;
using MediatR;

namespace Identity.Application.Features.Users.DeleteUser;

public sealed class DeleteUserHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, Result<DeleteUserResponse>>
{
    public async Task<Result<DeleteUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<DeleteUserResponse>.Failure(UserErrors.UserNotFound);
        }

        await userRepository.DeleteAsync(user, cancellationToken);
        var result = await userRepository.SaveChangesAsync(cancellationToken);

        if (result.IsFailure)
        {
            return Result<DeleteUserResponse>.Failure(result.Error);
        }

        return Result<DeleteUserResponse>.Success(new DeleteUserResponse());
    }
}