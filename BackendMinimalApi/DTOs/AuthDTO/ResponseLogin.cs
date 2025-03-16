﻿using System.Text.Json.Serialization;

namespace BackendMinimalApi.DTOs.AuthDTO;

public record ResponseLogin(
    [property: JsonPropertyName("status")] int StatusCode,
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);