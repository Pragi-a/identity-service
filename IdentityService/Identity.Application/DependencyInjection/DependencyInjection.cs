using FluentValidation;
using Identity.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly);
            cfg.AddOpenBehavior(
                typeof(ValidationBehaviour<,>)
            );
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(IApplicationAssemblyMarker).Assembly);
        return services;
    }
}