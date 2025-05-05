using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

internal class ProjectUsersService(RestClient restClient) : IProjectUsersService
{
    public async Task<string> Delete(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAll(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByProject(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByUser(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectUser> GetById(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Patch(string query, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Post(string query, object data)
    {
        throw new NotImplementedException();
    }
}
