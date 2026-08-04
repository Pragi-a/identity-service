using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security;
using Identity.Infrastructure.Security.Jwt;
using Identity.Infrastructure.Security.Password;
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

        //security
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        
        return services;
    }
}