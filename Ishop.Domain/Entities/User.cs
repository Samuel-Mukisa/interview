namespace Ishop.Domain.Entities;

public class User
{
    public string Username { get; set; } = string.Empty;
    public byte[] passwordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}