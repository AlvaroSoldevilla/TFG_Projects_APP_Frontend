using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

public class TaskBoardsService(RestClient restClient) : ITaskBoardsService
{
    private readonly string route = "task_boards";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<TaskBoard>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var taskBoards = await response.Content.ReadFromJsonAsync<ObservableCollection<TaskBoard>>(restClient._options);
        return taskBoards;
    }

    public async Task<ObservableCollection<TaskBoard>> GetAllTaskBoardsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var taskBoards = await response.Content.ReadFromJsonAsync<ObservableCollection<TaskBoard>>(restClient._options);
        return taskBoards;
    }

    public async Task<TaskBoard> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var taskBoard = await response.Content.ReadFromJsonAsync<TaskBoard>(restClient._options);
        return taskBoard;
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}
