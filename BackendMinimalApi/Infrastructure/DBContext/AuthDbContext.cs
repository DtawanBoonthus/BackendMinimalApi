using BackendMinimalApi.Models.AuthModel;
using Microsoft.EntityFrameworkCore;

namespace BackendMinimalApi.Infrastructure.DBContext;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{ 
    public DbSet<UserAccount> UserAccounts { get; set; }
}