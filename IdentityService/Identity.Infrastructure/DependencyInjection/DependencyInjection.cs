using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Security;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security;
using Identity.Infrastructure.Security.Jwt;
using Identity.Infrastructure.Security.Password;
using Identity.Infrastructure.Security.RefreshTokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Persistence
        services.AddDbContext<IdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("IdentityDb") ??
                                   throw new InvalidOperationException("Connection string 'IdentityDb' was not found.");
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        //security
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenHasher, Sha256RefreshTokenHasher>();
        
        return services;
    }
}