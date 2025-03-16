using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record RequestLogin(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password);