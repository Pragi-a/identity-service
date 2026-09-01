namespace Identity.Application.Interfaces.Security;

public interface IRefreshTokenGenerator
{
    string GenerateRefreshToken();
}