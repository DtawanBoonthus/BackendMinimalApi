namespace BackendMinimalApi.DTOs.AuthDTO;

public record TokenResult(string AccessToken, string RefreshToken);