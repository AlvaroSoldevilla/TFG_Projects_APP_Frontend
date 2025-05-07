using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectsService;

internal class ProjectsService(RestClient restClient) : IProjectsService
{
    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<Project>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ObservableCollection<Project>> GetAllProjectsByUser(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Project> GetById(int id)
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
