using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record RegisterResult(int UserId, string Username, TokenResult TokenResult);

public record RequestRegister(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password);
    
public record ResponseRegister(
    [property: JsonPropertyName("status")] int StatusCode,
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);