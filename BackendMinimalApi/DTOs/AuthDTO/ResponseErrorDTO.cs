using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record ResponseError(
    [property: JsonPropertyName("status")] int StatusCode,
    [property: JsonPropertyName("errorMessage")] string ErrorMessage);