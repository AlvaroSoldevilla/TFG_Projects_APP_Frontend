using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskSections;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService;

/*Implementation of the TaskSectionsService Interface*/
public class TaskSectionsService(RestClient restClient) : ITaskSectionsService
{
    private readonly string route = "task_sections";
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

    public async Task<List<TaskSection>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var taskSections = await response.Content.ReadFromJsonAsync<List<TaskSectionRead>>(restClient._options);
        return new List<TaskSection>(taskSections.Select(taskSection =>
        {
            return new TaskSection
            {
                Id = taskSection.Id,
                IdDefaultProgress = taskSection.IdDefaultProgress,
                Title = taskSection.Title,
                IdBoard = taskSection.IdBoard,
                Order = taskSection.Order
            };
        }).ToList());
    }

    public async Task<List<TaskSection>> GetAllTaskSectionsByTaskBoard(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/board/{id}");
        if (response == null)
        {
            return null;
        }
        var taskSections = await response.Content.ReadFromJsonAsync<List<TaskSectionRead>>(restClient._options);
        return new List<TaskSection>(taskSections.Select(taskSection =>
        {
            return new TaskSection
            {
                Id = taskSection.Id,
                IdDefaultProgress = taskSection.IdDefaultProgress,
                Title = taskSection.Title,
                IdBoard = taskSection.IdBoard,
                Order = taskSection.Order
            };
        }).ToList());
    }

    public async Task<TaskSection> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var taskSection = await response.Content.ReadFromJsonAsync<TaskSectionRead>(restClient._options);
        return new TaskSection
        {
            Id = taskSection.Id,
            IdDefaultProgress = taskSection.IdDefaultProgress,
            Title = taskSection.Title,
            IdBoard = taskSection.IdBoard,
            Order = taskSection.Order
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

    public async Task<TaskSection> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var taskSection = await response.Content.ReadFromJsonAsync<TaskSectionRead>(restClient._options);
        return new TaskSection
        {
            Id = taskSection.Id,
            IdDefaultProgress = taskSection.IdDefaultProgress,
            Title = taskSection.Title,
            IdBoard = taskSection.IdBoard,
            Order = taskSection.Order
        };
    }
}
