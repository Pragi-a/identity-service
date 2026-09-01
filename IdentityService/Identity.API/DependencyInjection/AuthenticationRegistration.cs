using System.Security.Cryptography;
using Identity.API.Common.Errors.ErrorToHttpError;
using Identity.API.Common.Errors.HttpErrorToResult;
using Identity.Application.Common.Errors.Helper;
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

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        var error = SecurityErrors.Unauthenticated;

                        var errorToHttpError =
                            context.HttpContext.RequestServices.GetRequiredService<IErrorToHttpMapper>();

                        var httpError = errorToHttpError.Map(error);

                        var httpErrorToResultMapper = context.HttpContext.RequestServices
                            .GetRequiredService<IHttpErrorToResultMapper>();

                        var result = httpErrorToResultMapper.Map(httpError);

                        await result.ExecuteAsync(context.HttpContext);
                    }
                };
            });
        return services;
    }
}