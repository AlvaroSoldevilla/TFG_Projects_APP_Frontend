using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService
{
    internal interface IUsersService : IService<AppUser>
    {
        Task<AppUser> GetUserByProject(string query, int id);
        Task<string> AuthenticateUser(string query, string email);
    }
}
