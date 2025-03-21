using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record LoginResult(int UserId, TokenResult TokenResult);

public record RequestLogin(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password);
    
public record ResponseLogin(
    [property: JsonPropertyName("status")] int StatusCode,
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);