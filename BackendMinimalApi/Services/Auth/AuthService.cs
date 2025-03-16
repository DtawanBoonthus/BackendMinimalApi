using BackendMinimalApi.DTOs.AuthDTO;
using BackendMinimalApi.Infrastructure.DBContext;
using BackendMinimalApi.Models.AuthModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendMinimalApi.Services.Auth;

public class AuthService(
    AuthDbContext context,
    IPasswordHasher<UserAccount> passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public async Task<LoginResult> LoginAsync(RequestLogin loginData)
    {
        var user = await context.UserAccounts.SingleOrDefaultAsync(u => u.Username == loginData.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginData.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var tokenResult = jwtTokenGenerator.GenerateToken(user);
        return new LoginResult(tokenResult);
    }
    
    public async Task<RegisterResult> RegisterAsync(RequestRegister registerData)
    {
        var existingUser = await context.UserAccounts.FirstOrDefaultAsync(u => u.Username == registerData.Username);
        
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists");
        }

        var newUser = new UserAccount
        {
            Username = registerData.Username,
            Password =  registerData.Password
        };
        
        newUser.Password = passwordHasher.HashPassword(newUser, registerData.Password);

        context.UserAccounts.Add(newUser);
        await context.SaveChangesAsync();
        
        return new RegisterResult(newUser.UserId, newUser.Username);
    }

    public Task<string> RefreshTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RevokeTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}