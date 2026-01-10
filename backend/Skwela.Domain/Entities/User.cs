using Skwela.Domain.Enums;

namespace Skwela.Domain.Entities;

public class User
{
    public Guid id { get; set; }
    public required string username { get; set; }
    public string password { get; set; } = default!;
    public UserRole role { get; set; }
    public string? refreshToken { get; set; }
    public DateTime refreshTokenExpiryTime { get; set; }

}