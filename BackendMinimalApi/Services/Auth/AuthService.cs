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
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginData.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var tokenResult = jwtTokenGenerator.GenerateToken(user);
        
        return new LoginResult(user.UserId ,tokenResult);
    }
    
    public async Task<RegisterResult> RegisterAsync(RequestRegister registerData)
    {
        var existingUser = await context.UserAccounts.SingleOrDefaultAsync(u => u.Username == registerData.Username);
        
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        var newUser = new UserAccount
        {
            Username = registerData.Username,
            Password =  registerData.Password
        };
        
        newUser.Password = passwordHasher.HashPassword(newUser, registerData.Password);

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            context.UserAccounts.Add(newUser);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
        
        var tokenResult = jwtTokenGenerator.GenerateToken(newUser);
        
        return new RegisterResult(newUser.UserId, newUser.Username, tokenResult);
    }

    public async Task<TokenResult> RefreshTokenAsync(int userId)
    {
        var user = await context.UserAccounts.SingleOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            throw new ArgumentException("Invalid user id");
        }
        
        return jwtTokenGenerator.GenerateToken(user);
    }
}