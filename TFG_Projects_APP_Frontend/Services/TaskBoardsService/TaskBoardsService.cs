using System.Collections.ObjectModel;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

internal class TaskBoardsService(RestClient restClient) : ITaskBoardsService
{
    public async Task<string> Delete(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<TaskBoard>> GetAll(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<ObservableCollection<TaskBoard>> GetAllTaskBoardsByProject(string query, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskBoard> GetById(string query, int id)
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
