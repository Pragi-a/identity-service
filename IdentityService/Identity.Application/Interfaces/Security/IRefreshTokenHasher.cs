namespace Identity.Application.Interfaces.Security;

public interface IRefreshTokenHasher
{
    string HashToken(string refreshToken);
    
}