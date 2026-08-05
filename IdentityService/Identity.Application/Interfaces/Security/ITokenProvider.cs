using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Security;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    
}