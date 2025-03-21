using BackendMinimalApi.DTOs.AuthDTO;

namespace BackendMinimalApi.Services.Auth;

public interface IRefreshTokenService
{
    public Task SaveRefreshTokenAsync(RefreshTokenData data);

    public Task<string?> GetDataRefreshTokenAsync(string key);

    public Task RevokeRefreshTokenAsync(string key);

    public TimeSpan GetRefreshTokenExpirationDays();
}