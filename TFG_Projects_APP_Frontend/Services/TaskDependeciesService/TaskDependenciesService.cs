using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskDependeciesService;

internal class TaskDependenciesService(RestClient restClient) : ITaskDependenciesService
{
    public Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ObservableCollection<TaskDependency>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ObservableCollection<TaskDependency>> GetAllTaskDependenciesByTask( int id)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDependency> GetById(int id)
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
