namespace Identity.API.Common.MiddleWare;

public sealed class CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var correlationId = Guid.NewGuid().ToString();

        httpContext.Response.Headers.Append(CorrelationIdHeaderName, correlationId);

        using (logger.BeginScope(new[]
               {
                   new KeyValuePair<string, object>("CorrelationId", correlationId)
               }))
        {
            await next(httpContext);
        }
    }
}