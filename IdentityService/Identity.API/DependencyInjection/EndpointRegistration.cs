using Identity.API.Common.Filters.EndpointFilters;
using Identity.API.Features.Login;
using Identity.API.Features.Me;
using Identity.API.Features.Refresh;
using Identity.API.Features.Register;
using Identity.API.Features.Roles.UpdateRolePermissions;
using Identity.API.Features.Users.CreateUser;
using Identity.API.Features.Users.DeactivateUser;
using Identity.API.Features.Users.DeleteUser;
using Identity.API.Features.Users.GetUserById;
using Identity.API.Features.Users.GetUsers;
using Identity.API.Features.Users.UpdateUser;

namespace Identity.API.DependencyInjection;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.AddEndpointFilter<ApiResultEndpointFilter>();

        app.MapRegisterEndpoint();
        app.MapLoginEndpoint();
        app.MapMeEndpoint();
        app.MapRefreshEndpoint();

        app.MapCreateUserEndpoint();
        app.MapGetUsersEndpoint();
        app.MapGetUserByIdEndpoint();
        app.MapUpdateUserEndpoint();
        app.MapDeactivateUserEndpoint();
        app.MapDeleteUserEndpoint();


        app.MapUpdateRolePermissions();
        return app;
    }
}