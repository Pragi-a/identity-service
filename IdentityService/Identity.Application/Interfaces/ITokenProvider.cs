using Identity.Domain.Entities.Users;

namespace Identity.Application.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    
}