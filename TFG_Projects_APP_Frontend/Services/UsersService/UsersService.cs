using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

internal class UsersService(RestClient restClient) : IUsersService
{
    public Task<string> AuthenticateUser(string query, string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> Delete(string query, int id)
    {
        throw new NotImplementedException();
    }

    public Task<ObservableCollection<AppUser>> GetAll(string query)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetById(string query, int id)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetUserByProject(string query, int id)
    {
        throw new NotImplementedException();
    }

    public Task<string> Patch(string query, object data)
    {
        throw new NotImplementedException();
    }

    public Task<string> Post(string query, object data)
    {
        throw new NotImplementedException();
    }
}
