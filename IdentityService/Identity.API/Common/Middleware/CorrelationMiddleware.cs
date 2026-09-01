using Identity.API.Common.Correlation;

namespace Identity.API.Common.MiddleWare;

public sealed class CorrelationMiddleware(
    RequestDelegate next,
    ILogger<CorrelationMiddleware> logger
)
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext httpContext, ICorrelationContextInitializer correlationContextInitializer)
    {
        var correlationId = Guid.NewGuid();

        correlationContextInitializer.SetCorrelationId(correlationId);
        httpContext.Response.Headers.Append(CorrelationIdHeaderName, correlationId.ToString());

        using (logger.BeginScope(new[]
               {
                   new KeyValuePair<string, object>("CorrelationId", correlationId)
               }))
        {
            await next(httpContext);
        }
    }
}