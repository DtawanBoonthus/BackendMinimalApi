using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record RegisterResult(
    [property: JsonPropertyName("userId")] int UserId,
    [property: JsonPropertyName("username")] string Username);