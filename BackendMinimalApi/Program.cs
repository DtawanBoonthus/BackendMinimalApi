using BackendMinimalApi.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();