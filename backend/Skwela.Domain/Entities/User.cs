using Skwela.Domain.Enums;

namespace Skwela.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }
}