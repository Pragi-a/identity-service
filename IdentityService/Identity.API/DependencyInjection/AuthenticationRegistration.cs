using System.Security.Cryptography;
using Identity.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.DependencyInjection;

public static class AuthenticationRegistration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ??
                                 throw new InvalidOperationException("JWT Configuration is missing");

                var publicKeyPem = File.ReadAllText(jwtOptions.PublicKeyPath);

                var rsa = RSA.Create();

                rsa.ImportFromPem(publicKeyPem);

                var publicKey = new RsaSecurityKey(rsa);

                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = publicKey,

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });

        services.AddAuthorization();
        return services;
    }
}