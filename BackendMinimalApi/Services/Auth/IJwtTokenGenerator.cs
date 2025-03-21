using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Models.AuthModel;

namespace BackendMinimalApi.Services.Auth;

public interface IJwtTokenGenerator
{
    public TokenResult GenerateToken(UserAccount user);

    public string GenerateRefreshToken();
}