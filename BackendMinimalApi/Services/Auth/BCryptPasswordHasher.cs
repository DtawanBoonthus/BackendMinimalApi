using Microsoft.AspNetCore.Identity;

namespace BackendMinimalApi.Services.Auth;

public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        bool verified = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        return verified ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}