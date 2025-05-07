using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

internal class ProjectUsersService(RestClient restClient) : IProjectUsersService
{
    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByProject(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByUser(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectUser> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Patch(int id, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Post(object data)
    {
        throw new NotImplementedException();
    }
}
