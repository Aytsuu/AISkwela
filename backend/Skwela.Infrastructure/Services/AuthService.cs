using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Skwela.Application.Interfaces;
using Skwela.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Skwela.Domain.Entities;
using Skwela.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;


namespace Skwela.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IConfiguration config, ILogger<AuthService> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        _logger.LogInformation("Attempting to login user: {Username}", username);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.refreshToken = refreshToken;
        user.refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(20);
        await _context.SaveChangesAsync();

        return new AuthResponse(accessToken, refreshToken);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.refreshToken == refreshToken);

        if (user == null || user.refreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.refreshToken = newRefreshToken;
        user.refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(20);
        await _context.SaveChangesAsync();

        return new AuthResponse(accessToken, refreshToken);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32]; 
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.username),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                int.Parse(_config["Jwt:ExpiresInMinutes"]!)
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> SignupAsync(string username, string password)
    {
        _logger.LogInformation("Attempting to sign up user: {Username}", username);

        var existingUser = await _context.Users
             .FirstOrDefaultAsync(u => u.username == username);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            id = Guid.NewGuid(),
            username = username,
            password = hashedPassword,
            role = UserRole.Student
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.id.ToString();
    }

}