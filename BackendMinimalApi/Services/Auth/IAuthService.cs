using BackendMinimalApi.DTOs.AuthDTO;

namespace BackendMinimalApi.Services.Auth;

public interface IAuthService
{
    public Task<LoginResult> LoginAsync(RequestLogin loginData);

    public Task<RegisterResult> RegisterAsync(RequestRegister registerData);

    public Task<TokenResult> RefreshTokenAsync(int userId);
}