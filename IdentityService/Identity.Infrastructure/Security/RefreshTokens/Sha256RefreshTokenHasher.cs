using System.Security.Cryptography;
using System.Text;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Security;

namespace Identity.Infrastructure.Security.RefreshTokens;

public sealed class Sha256RefreshTokenHasher : IRefreshTokenHasher
{
    public string HashToken(string refreshToken)
    {
        if (refreshToken == string.Empty || refreshToken.Length == 0 || string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Invalid refresh token");


        var bytes = Encoding.UTF8.GetBytes(refreshToken);

        var hashedToken = SHA256.HashData(bytes);

        return Convert.ToBase64String(hashedToken);
    }
}