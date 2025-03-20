using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Models.AuthModel;
using Microsoft.IdentityModel.Tokens;

namespace BackendMinimalApi.Services.Auth;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public TokenResult GenerateToken(UserAccount user)
    {
        var secretKey = configuration.GetValue<string>("Jwt:Secret") ??
                        throw new Exception("JWT Secret is not configured.");
        var issuer = configuration.GetValue<string>("Jwt:Issuer");
        var audience = configuration.GetValue<string>("Jwt:Audience");
        var expirationMinutes = configuration.GetValue("Jwt:ExpirationMinutes", 60);

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        return new TokenResult(jwtToken, refreshToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}