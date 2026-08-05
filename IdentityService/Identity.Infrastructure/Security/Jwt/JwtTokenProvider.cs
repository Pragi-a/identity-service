using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Security.Jwt;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly RsaSecurityKey _privateKey;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;


        if (!File.Exists(_jwtOptions.PrivateKeyPath))
        {
            throw new FileNotFoundException($"JWT private key '{_jwtOptions.PrivateKeyPath}' was not found.");
        }

        var privateKey = File.ReadAllText(_jwtOptions.PrivateKeyPath);

        var rsa = RSA.Create();

        rsa.ImportFromPem(privateKey);

        _privateKey = new RsaSecurityKey(rsa);
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var signingCredentials = new SigningCredentials(this._privateKey, SecurityAlgorithms.RsaSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(this._jwtOptions.ExpiryMinutes);

        var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials
            );

        var handler = new JwtSecurityTokenHandler();
        
        return handler.WriteToken(token);
    }
}   