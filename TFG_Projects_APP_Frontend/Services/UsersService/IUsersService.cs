using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

/*Inherits IService and adds model specific methods*/
public interface IUsersService : IService<AppUser>
{
    /*used to get all users from a project*/
    Task<List<AppUser>> GetUsersByProject(int id);
    /*used to authenticate a user*/
    Task<AppUser> AuthenticateUser(object data);
    /*Used to get a user by their email*/
    Task<AppUser> GetUserByEmail(string email);
}
