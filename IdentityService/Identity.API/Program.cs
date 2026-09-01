using Identity.API.Common.Errors.ErrorToHttpError;
using Identity.API.Common.Errors.HttpErrorToResult;
using Identity.API.Common.Results.SuccessToResult;
using Identity.API.DependencyInjection;
using Identity.API.Extensions;
using Identity.Application.DependencyInjection;
using Identity.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<ISuccessToResultMapper, SuccessToResultMapper>();
builder.Services.AddScoped<IErrorToHttpMapper, ErrorToHttpMapper>();
builder.Services.AddScoped<IHttpErrorToResultMapper, HttpErrorToResultMapper>();

builder.Services.AddAuthorization();

builder.Logging.AddJsonConsole(options => { options.IncludeScopes = true; });

var app = builder.Build();
await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
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