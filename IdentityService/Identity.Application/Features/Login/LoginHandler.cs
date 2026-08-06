using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Features.Login;

public class LoginHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider provider,
    IRefreshTokenRepository refreshTokenRepository,
    IRefreshTokenHasher refreshTokenHasher,
    IRefreshTokenGenerator refreshTokenGenerator)
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


        var accessToken = provider.AccessTokenResult(user);

        var refreshTokenString = refreshTokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = refreshTokenHasher.HashToken(refreshTokenString);

        var refreshToken = RefreshToken.Create(
            hashedRefreshToken, 
            user.Id, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(30));
        
        await refreshTokenRepository.AddRefreshToken(refreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        
        return Result<LoginResponse>.Success(new LoginResponse(accessToken, refreshTokenString, string.Empty));
    }
}