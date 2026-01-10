namespace Skwela.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);
    Task<string> SignupAsync(string username, string password);
}