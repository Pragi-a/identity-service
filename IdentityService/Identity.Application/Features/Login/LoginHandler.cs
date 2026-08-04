using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Features.Login;

public class LoginHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenProvider provider)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            return Result<LoginResponse>.Failure(UserErrors.InvalidCredentials);

        var isValidPassword = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValidPassword)
            return Result<LoginResponse>.Failure(UserErrors.InvalidCredentials);


        var accessToken = provider.GenerateAccessToken(user);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken, string.Empty, string.Empty));
    }
}