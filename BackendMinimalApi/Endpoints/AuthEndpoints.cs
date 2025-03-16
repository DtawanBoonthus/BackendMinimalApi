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
                var loginResult = await authService.LoginAsync(loginData);
                return string.IsNullOrEmpty(loginResult.Token.AccessToken)
                    ? Results.BadRequest(new ResponseError(400, "Login failed"))
                    : Results.Ok(new ResponseLogin(200, loginResult.Token.AccessToken, loginResult.Token.RefreshToken));
            })
            .WithName("Login")
            .WithTags("Auth");

        group.MapPost("/register", async (IAuthService authService, RequestRegister registerData) =>
            {
                try
                {
                    var registerResult = await authService.RegisterAsync(registerData);
                    return Results.Created($"/auth/{registerResult.UserId}",
                        new ResponseRegister(201, registerResult.Username));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseError(400, ex.Message));
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