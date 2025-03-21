using BackendMinimalApi.DTOs.AuthDTO;
using Microsoft.Extensions.Caching.Distributed;

namespace BackendMinimalApi.Services.Auth;

public class RedisRefreshTokenService(IDistributedCache cache, IConfiguration configuration) : IRefreshTokenService
{
    public async Task SaveRefreshTokenAsync(RefreshTokenData data)
    {
        try
        {
            await cache.SetStringAsync(data.RefreshToken, data.UserId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = data.Expiration
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<string?> GetDataRefreshTokenAsync(string key)
    {
        try
        {
            return await cache.GetStringAsync(key);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task RevokeRefreshTokenAsync(string key)
    {
        try
        {
            var dataRefreshToken = await cache.GetStringAsync(key);
            
            if (dataRefreshToken == null)
            {
                throw new KeyNotFoundException("Refresh token not found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
        try
        {
            await cache.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public TimeSpan GetRefreshTokenExpirationDays()
    {
        int expirationRefreshToken = configuration.GetValue("RefreshExpirationDays", 7);
        return TimeSpan.FromDays(expirationRefreshToken);
    }
}