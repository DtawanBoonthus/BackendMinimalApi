using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record RequestRegister(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password);