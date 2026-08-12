using Identity.Application.Common.Errors;
using MediatR;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Domain.ValueObjects;

namespace Identity.Application.Features.Users.UpdateUser;

public sealed class UpdateUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result<UpdateUserResponse>.Failure(UserErrors.UserNotFound);

        var email = new Email(request.Email);
        var firstName = new Name(request.FirstName);
        var lastName = new Name(request.LastName);

        user.UpdateFirstName(firstName);
        user.UpdateLastName(lastName);
        user.UpdateEmail(email);
        user.ReplaceRoles(request.RoleIds);

        var result = await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result.IsFailure)
        {
            Result<UpdateUserResponse>.Failure(result.Error);
        }


        return Result<UpdateUserResponse>.Success(new UpdateUserResponse(request.UserId, user.FirstName.Value,
            user.LastName.Value,
            user.Email.Value));
    }
}