using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Services.Auth;

namespace BackendMinimalApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth");

        group.MapPost("/login", async (IAuthService authService, RequestLogin loginData) =>
            {
                try
                {
                    var result = await authService.LoginAsync(loginData);
                    return Results.Ok(new ResponseLogin(200, result.Token.AccessToken, result.Token.RefreshToken));
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

        group.MapPost("/register", async (IAuthService authService, RequestRegister registerData) =>
            {
                try
                {
                    var result = await authService.RegisterAsync(registerData);
                    return Results.Ok(new ResponseRegister(200, result.TokenResult.AccessToken,
                        result.TokenResult.RefreshToken));
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

        group.MapPost("/refresh", async (IAuthService authService, string token) =>
            {
                var newToken = await authService.RefreshTokenAsync(token);
                return string.IsNullOrEmpty(newToken)
                    ? Results.BadRequest(new ResponseError(400, "Invalid token"))
                    : Results.Ok(new { Token = newToken });
            })
            .WithName("RefreshToken")
            .WithTags("Auth");

        group.MapPost("/revoke", async (IAuthService authService, string token) =>
            {
                var success = await authService.RevokeTokenAsync(token);
                return success
                    ? Results.Ok(new { status = 200, message = "Token revoked" })
                    : Results.BadRequest(new ResponseError(400, "Token could not be revoked"));
            })
            .WithName("RevokeToken")
            .WithTags("Auth");

        return endpoints;
    }
}