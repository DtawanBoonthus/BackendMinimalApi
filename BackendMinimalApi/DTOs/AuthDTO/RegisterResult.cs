namespace BackendMinimalApi.DTOs.AuthDTO;

public record RegisterResult(int UserId, string Username, TokenResult TokenResult);