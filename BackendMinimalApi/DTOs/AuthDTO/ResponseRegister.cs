using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record ResponseRegister(
    [property: JsonPropertyName("status")] int StatusCode,
    [property: JsonPropertyName("username")] string Username);