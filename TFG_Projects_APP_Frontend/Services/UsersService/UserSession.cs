using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

/*Holds the data of the logged in user necessary for certain actions and refreshing the API token*/
public class UserSession
{
    public AppUser User { get; set; } = new() { Id = 1, Username = "Admin", Email = "admin@test.com"};
    public string Token { get; set; } = "";
}
