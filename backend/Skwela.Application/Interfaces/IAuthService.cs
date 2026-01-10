namespace Skwela.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string username, string password);
    Task<string> SignupAsync(string username, string password);
    Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken);
}