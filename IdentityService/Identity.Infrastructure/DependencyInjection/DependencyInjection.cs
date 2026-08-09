using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Application.Interfaces.Repositories.Queries;
using Identity.Application.Interfaces.Security;
using Identity.Infrastructure.Authorization;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Queries;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Persistence.Seed;
using Identity.Infrastructure.Security.Jwt;
using Identity.Infrastructure.Security.Password;
using Identity.Infrastructure.Security.RefreshTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Authorization
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        //Persistence
        services.AddDbContext<IdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("IdentityDb") ??
                                   throw new InvalidOperationException("Connection string 'IdentityDb' was not found.");
            options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<IUserAuthorizationQueries, UserAuthorizationQueries>();

        //Persistence - Queries
        services.AddScoped<IUserQueries, UserQueries>();

        //security
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenHasher, Sha256RefreshTokenHasher>();

        return services;
    }
}