using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace BackendMinimalApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration.GetValue<string>("Jwt:Secret") ??
                    throw new Exception("JWT Secret is not configured."))),
                ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IServiceCollection AddNSwagOpenApiDocument(this IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "Backend Minimal API";
            config.Title = "MinimalAPI v1";
            config.Version = "v1";

            config.AddSecurity("Bearer", new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization",
                Description = "Input your JWT token like: Bearer {your_token}"
            });

            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });

        return services;
    }

    public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis") ??
                                    throw new ArgumentException("Redis is not configured.");
        });

        return services;
    }
}