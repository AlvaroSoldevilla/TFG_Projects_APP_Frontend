using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

public interface IUsersService : IService<AppUser>
{
    Task<List<AppUser>> GetUsersByProject(int id);
    Task<AppUser> AuthenticateUser(object data);
}
