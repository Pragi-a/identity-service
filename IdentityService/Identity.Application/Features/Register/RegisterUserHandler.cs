using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Features.Register;

public class RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        
        //check if the email exists
        var emailExists = await userRepository.ExistsByEmailAsync(email, cancellationToken);
        if (emailExists)
            return Result<RegisterUserResponse>.Failure(UserErrors.EmailAlreadyExists);

        //hash the password
        var hashedPassword = passwordHasher.HashPassword(request.Password);

        // create the user
        var user = new User(email, hashedPassword, request.FirstName, request.LastName);

        //save the user
        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        //return success
        return Result<RegisterUserResponse>.Success(new RegisterUserResponse(user.Id));
    }
}