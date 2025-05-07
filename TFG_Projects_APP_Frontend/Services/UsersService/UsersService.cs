using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

internal class UsersService(RestClient restClient) : IUsersService
{
    public Task<string> AuthenticateUser(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ObservableCollection<AppUser>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetUserByProject(int id)
    {
        throw new NotImplementedException();
    }

    public Task<string> Patch(int id, object data)
    {
        throw new NotImplementedException();
    }

    public Task<string> Post(object data)
    {
        throw new NotImplementedException();
    }
}
