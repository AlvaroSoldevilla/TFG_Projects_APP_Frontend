using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskDependecies;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskDependeciesService;

/*Implementation of the TaskDependenciesService Interface*/
public class TaskDependenciesService(RestClient restClient) : ITaskDependenciesService
{
    private readonly string route = "task_dependencies";
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

    public async Task<List<TaskDependency>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var taskDependencies = await response.Content.ReadFromJsonAsync<List<TaskDependencyRead>>(restClient._options);
        return new List<TaskDependency>(taskDependencies.Select(taskDependency =>
        {
            return new TaskDependency
            {
                Id = taskDependency.Id,
                IdTask = taskDependency.IdTask,
                IdDependsOn = taskDependency.IdDependsOn,
                UnlockAt = taskDependency.UnlockAt
            };
        }).ToList());
    }

    public async Task<List<TaskDependency>> GetAllTaskDependenciesByTask( int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/task/{id}");
        if (response == null)
        {
            return null;
        }
        var taskDependencies = await response.Content.ReadFromJsonAsync<List<TaskDependencyRead>>(restClient._options);
        return new List<TaskDependency>(taskDependencies.Select(taskDependency =>
        {
            return new TaskDependency
            {
                Id = taskDependency.Id,
                IdTask = taskDependency.IdTask,
                IdDependsOn = taskDependency.IdDependsOn,
                UnlockAt = taskDependency.UnlockAt
            };
        }).ToList());
    }

    public async Task<TaskDependency> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var taskDependency = await response.Content.ReadFromJsonAsync<TaskDependencyRead>(restClient._options);
        return new TaskDependency
        {
            Id = taskDependency.Id,
            IdTask = taskDependency.IdTask,
            IdDependsOn = taskDependency.IdDependsOn,
            UnlockAt = taskDependency.UnlockAt
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

    public async Task<TaskDependency> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var taskDependency = await response.Content.ReadFromJsonAsync<TaskDependencyRead>(restClient._options);
        return new TaskDependency
        {
            Id = taskDependency.Id,
            IdTask = taskDependency.IdTask,
            IdDependsOn = taskDependency.IdDependsOn,
            UnlockAt = taskDependency.UnlockAt
        };
    }
}
