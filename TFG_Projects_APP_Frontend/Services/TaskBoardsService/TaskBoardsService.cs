using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskBoards;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskBoardsService;

/*Implementation of the TaskBoardsService Interface*/
public class TaskBoardsService(RestClient restClient) : ITaskBoardsService
{
    private readonly string route = "task_boards";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<List<TaskBoard>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var taskBoards = await response.Content.ReadFromJsonAsync<List<TaskBoardRead>>(restClient._options);
        return new List<TaskBoard>(taskBoards.Select(taskBoard =>
        {
            return new TaskBoard
            {
                Id = taskBoard.Id,
                Title = taskBoard.Title,
                Description = taskBoard.Description,
                IdProject = taskBoard.IdProject
            };
        }).ToList());
    }

    public async Task<List<TaskBoard>> GetAllTaskBoardsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        if (response == null)
        {
            return null;
        }
        var taskBoards = await response.Content.ReadFromJsonAsync<List<TaskBoardRead>>(restClient._options);
        return new List<TaskBoard>(taskBoards.Select(taskBoard =>
        {
            return new TaskBoard
            {
                Id = taskBoard.Id,
                Title = taskBoard.Title,
                Description = taskBoard.Description,
                IdProject = taskBoard.IdProject
            };
        }).ToList());
    }

    public async Task<TaskBoard> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var taskBoard = await response.Content.ReadFromJsonAsync<TaskBoardRead>(restClient._options);
        return new TaskBoard
        {
            Id = taskBoard.Id,
            Title = taskBoard.Title,
            Description = taskBoard.Description,
            IdProject = taskBoard.IdProject
        };
    }

    public async Task<string> Patch(int id, object data)
    {
        HttpResponseMessage response = await restClient.PatchAsync(route, id, data);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<TaskBoard> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var taskBoard = await response.Content.ReadFromJsonAsync<TaskBoardRead>(restClient._options);
        return new TaskBoard
        {
            Id = taskBoard.Id,
            Title = taskBoard.Title,
            Description = taskBoard.Description,
            IdProject = taskBoard.IdProject
        };
    }
}
