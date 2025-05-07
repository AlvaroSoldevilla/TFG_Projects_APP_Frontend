using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

internal class TaskBoardsService(RestClient restClient) : ITaskBoardsService
{
    public async Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<TaskBoard>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<TaskBoard>> GetAllTaskBoardsByProject(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskBoard> GetById(int id)
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
