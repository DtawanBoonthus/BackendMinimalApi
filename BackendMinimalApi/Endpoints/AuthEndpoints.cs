using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Services.Auth;

namespace BackendMinimalApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth");

        group.MapPost("/login", async (IAuthService authService, IRefreshTokenService refreshTokenService, RequestLogin loginData) =>
            {
                try
                {
                    var result = await authService.LoginAsync(loginData);
                    await refreshTokenService.SaveRefreshTokenAsync(new RefreshTokenData(result.UserId, result.TokenResult.RefreshToken, refreshTokenService.GetRefreshTokenExpirationDays()));
                    return Results.Ok(new ResponseLogin(200, result.TokenResult.AccessToken, result.TokenResult.RefreshToken));
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.BadRequest(new ResponseError(400, ex.Message));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseError(500, ex.Message));
                }
            })
            .WithName("Login")
            .WithTags("Auth");

        group.MapPost("/register", async (IAuthService authService, IRefreshTokenService refreshTokenService, RequestRegister registerData) =>
            {
                try
                {
                    var result = await authService.RegisterAsync(registerData);
                    await refreshTokenService.SaveRefreshTokenAsync(new RefreshTokenData(result.UserId, result.TokenResult.RefreshToken, refreshTokenService.GetRefreshTokenExpirationDays()));
                    return Results.Ok(new ResponseRegister(200, result.TokenResult.AccessToken, result.TokenResult.RefreshToken));
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new ResponseError(10001, ex.Message));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseError(500, ex.Message));
                }
            })
            .WithName("Register")
            .WithTags("Auth");
        
        return endpoints;
    }
}