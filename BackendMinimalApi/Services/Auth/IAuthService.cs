using BackendMinimalApi.DTOs.AuthDTO;

namespace BackendMinimalApi.Services.Auth;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(RequestLogin loginData);
    
    Task<RegisterResult> RegisterAsync(RequestRegister registerData);
    
    Task<string> RefreshTokenAsync(string token);
    
    Task<bool> RevokeTokenAsync(string token);
}