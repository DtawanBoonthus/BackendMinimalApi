using BackendMinimalApi.Endpoints;
using BackendMinimalApi.Extensions;
using BackendMinimalApi.Infrastructure.DBContext;
using BackendMinimalApi.Models.AuthModel;
using BackendMinimalApi.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddStackExchangeRedis(builder.Configuration);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));

builder.Services.AddSingleton<IRefreshTokenService, RedisRefreshTokenService>();
builder.Services.AddSingleton<IPasswordHasher<UserAccount>, BCryptPasswordHasher<UserAccount>>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddNSwagOpenApiDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "BackendMinimalAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapToken();
app.MapTestAuthorization();

app.Run();