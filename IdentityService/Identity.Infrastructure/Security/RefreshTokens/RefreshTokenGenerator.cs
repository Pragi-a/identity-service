using System.Security.Cryptography;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Security;

namespace Identity.Infrastructure.Security.RefreshTokens;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int RefreshTokenSize = 32;
    
    public string GenerateRefreshToken()
    {
        var bytes = new Byte[RefreshTokenSize];
        
        RandomNumberGenerator.Fill(bytes);
        
        return Convert.ToBase64String(bytes);
    }
}