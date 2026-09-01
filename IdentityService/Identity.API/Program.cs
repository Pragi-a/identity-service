using System.Text.Json;
using Identity.API.Authorization;
using Identity.API.Common.Correlation;
using Identity.API.Common.Errors.ErrorToHttpError;
using Identity.API.Common.Errors.HttpErrorToResult;
using Identity.API.Common.Exceptions;
using Identity.API.Common.MiddleWare;
using Identity.API.Common.Results.SuccessToResult;
using Identity.API.DependencyInjection;
using Identity.API.Extensions;
using Identity.Application.DependencyInjection;
using Identity.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<ISuccessToResultMapper, SuccessToResultMapper>();
builder.Services.AddScoped<IErrorToHttpMapper, ErrorToHttpMapper>();
builder.Services.AddScoped<IHttpErrorToResultMapper, HttpErrorToResultMapper>();
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//Interface Segregated Dependency Injection
builder.Services.AddScoped<CorrelationContext>();
builder.Services.AddScoped<ICorrelationContext>(serviceProvider =>
    serviceProvider.GetRequiredService<CorrelationContext>());
builder.Services.AddScoped<ICorrelationContextInitializer>(serviceProvider =>
    serviceProvider.GetRequiredService<CorrelationContext>());
//....

builder.Services.AddAuthorization();
var jsonWriterOptions = new JsonWriterOptions
{
    Indented = true
};
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.JsonWriterOptions = jsonWriterOptions;
});

var app = builder.Build();
await app.InitializeDatabaseAsync();

app.UseMiddleware<CorrelationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.Run();