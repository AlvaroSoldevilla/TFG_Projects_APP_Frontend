using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

public class UserSession
{
    public AppUser User { get; set; } = new() { Id = 1, Username = "Admin", Email = "admin@test.com"};
}
