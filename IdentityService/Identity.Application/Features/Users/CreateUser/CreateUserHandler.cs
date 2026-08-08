using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Features.Users.CreateUser;

public sealed class CreateUserHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher
) : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var emailInUse = await userRepository.ExistsByEmailAsync(email, cancellationToken);

        if (emailInUse)
            return Result<CreateUserResponse>.Failure(UserErrors.EmailAlreadyExists);

        var roles = await roleRepository.GetByIdsAsync(request.RoleIds, cancellationToken);

        if (roles.Count != request.RoleIds.Distinct().Count())
            return Result<CreateUserResponse>.Failure(UserErrors.InvalidRole);


        var passwordHash = passwordHasher.HashPassword(request.Password);


        var user = new User(email, passwordHash, request.FirstName, request.LastName);

        user.ReplaceRoles(request.RoleIds);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return Result<CreateUserResponse>.Success(new CreateUserResponse(user.Id, user.Email.Value));
    }
}