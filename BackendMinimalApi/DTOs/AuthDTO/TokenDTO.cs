namespace BackendMinimalApi.DTOs.AuthDTO;

public record TokenResult(string AccessToken, string RefreshToken);

public record RefreshTokenData(int UserId, string RefreshToken, TimeSpan Expiration);

public record RequestRefreshToken(string RefreshToken);

public record ResponseRefreshToken(int StatusCode, string AccessToken, string RefreshToken);