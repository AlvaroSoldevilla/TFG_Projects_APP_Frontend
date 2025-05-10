using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService
{
    internal interface IUsersService : IService<AppUser>
    {
        Task<ObservableCollection<AppUser>> GetUsersByProject(int id);
        Task<string> AuthenticateUser(string email, object data);
    }
}
