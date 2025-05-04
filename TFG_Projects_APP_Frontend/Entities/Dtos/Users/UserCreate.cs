namespace TFG_Projects_APP_Frontend.Entities.Dtos.Users;

internal class UserCreate
{
    public string Password { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Email { get; set; }
}
