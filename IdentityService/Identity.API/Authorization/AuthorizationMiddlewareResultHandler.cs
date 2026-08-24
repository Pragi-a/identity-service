using Identity.API.Common.Errors.ErrorToHttpError;
using Identity.API.Common.Errors.HttpErrorToResult;
using Identity.Application.Common.Errors.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Identity.API.Authorization;

public sealed class AuthorizationMiddlewareResultHandler(
    IErrorToHttpMapper errToHttpMapper,
    IHttpErrorToResultMapper httpErrToResultMapper)
    : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await next(context);
            return;
        }

        var error = authorizeResult.Challenged ? SecurityErrors.Unauthenticated : SecurityErrors.Forbidden;
        var httpError = errToHttpMapper.Map(error);
        var result = httpErrToResultMapper.Map(httpError);

        await result.ExecuteAsync(context);
    }
}