using Identity.API.Features.Login;
using Identity.API.Features.Me;
using Identity.API.Features.Refresh;
using Identity.API.Features.Register;

namespace Identity.API.DependencyInjection;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRegisterEndpoint();
        app.MapLoginEndpoint();
        app.MapMeEndpoint();
        app.MapRefreshEndpoint();
        return app;
    }
}