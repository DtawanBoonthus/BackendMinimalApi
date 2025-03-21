using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Services.Auth;

namespace BackendMinimalApi.Endpoints;

public static class TokenEndpoints
{
    public static IEndpointRouteBuilder MapToken(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/token");

        group.MapPost("/refreshToken", async (IAuthService authService, IRefreshTokenService refreshTokenService, IConfiguration configuration,
                    RequestRefreshToken refreshTokenData) =>
                {
                    try
                    {
                        var resultData = await refreshTokenService.GetDataRefreshTokenAsync(refreshTokenData.RefreshToken);

                        if (string.IsNullOrEmpty(resultData) || !int.TryParse(resultData, out int userId))
                        {
                            return Results.NotFound(new ResponseError(404, "Refresh token not found"));
                        }

                        var newTokenResult = await authService.RefreshTokenAsync(userId);
                        var newRefreshTokenData = new RefreshTokenData(userId, newTokenResult.RefreshToken, refreshTokenService.GetRefreshTokenExpirationDays());
                        await refreshTokenService.SaveRefreshTokenAsync(newRefreshTokenData);
                        await refreshTokenService.RevokeRefreshTokenAsync(refreshTokenData.RefreshToken);

                        return Results.Ok(new ResponseRefreshToken(200, newTokenResult.AccessToken, newTokenResult.RefreshToken));
                    }
                    catch (KeyNotFoundException ex)
                    {
                        return Results.NotFound(new ResponseError(404, ex.Message));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new ResponseError(500, ex.Message));
                    }
                })
            .WithName("RefreshToken")
            .WithTags("Token");
        
        group.MapPost("/revokeRefreshToken", async (IRefreshTokenService refreshTokenService, RequestRefreshToken request) =>
            {
                try
                {
                    await refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);
                    return Results.Ok(new { Message = "Refresh token revoked successfully." });
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new ResponseError(404, ex.Message));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseError(500, ex.Message));
                }
            })
            .WithName("RevokeRefreshToken")
            .WithTags("Token");

        return endpoints;
    }
}