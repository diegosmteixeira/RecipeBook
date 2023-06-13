using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RecipeBook.API.Filters;
using RecipeBook.API.Filters.CustomAuthorize;
using RecipeBook.API.Filters.Swagger;
using RecipeBook.API.Middleware;
using RecipeBook.API.WebSockets;
using RecipeBook.Application;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Domain.Extension;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.Migrations;
using RecipeBook.Infrastructure.Repository.RepositoryAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<HashidsOperationFilter>();
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "RecipeBook API", Version = "1.0" });
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using Bearer Scheme. Example: \"Authorization: Bearer {token}\""
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionsFilter)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperConfiguration(provider.GetService<IHashids>()));
}).CreateMapper());

builder.Services.AddScoped<IAuthorizationHandler, UserLoggedHandler>();
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("UserLogged", policy => policy.Requirements.Add(new UserLoggedRequirement()));
});
builder.Services.AddScoped<AuthorizationAttribute>();

builder.Services.AddSignalR();

builder.Services.AddHealthChecks().AddDbContextCheck<RecipeBookContext>();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DatabaseUpdate();

app.UseMiddleware<CultureMiddleware>();

app.MapHub<Socket>("/connect");

app.Run();

void DatabaseUpdate()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<RecipeBookContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var connection = builder.Configuration.GetConnectionString();
        var databaseName = builder.Configuration.GetDatabaseName();
        Database.CreateDatabase(connection, databaseName);

        app.MigrateDatabase();
    }
}

public partial class Program { }