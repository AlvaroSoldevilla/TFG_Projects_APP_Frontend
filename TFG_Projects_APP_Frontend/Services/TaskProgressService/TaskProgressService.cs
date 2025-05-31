using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Dtos.TaskProgress;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService;

public class TaskProgressService(RestClient restClient) : ITaskProgressService
{
    private readonly string route = "task_progress";
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

    public async Task<List<TaskProgress>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        if (response == null)
        {
            return null;
        }
        var taskProgress = await response.Content.ReadFromJsonAsync<List<TaskProgressRead>>(restClient._options);
        return new List<TaskProgress>(taskProgress.Select(dto =>
        {
            return new TaskProgress
            {
                Id = dto.Id,
                IdSection = dto.IdSection,
                Title = dto.Title,
                ModifiesProgress = dto.ModifiesProgress,
                ProgressValue = dto.ProgressValue,
                Order = dto.Order
            };
        }).ToList());
    }

    public async Task<List<TaskProgress>> getAlltaskProgressByTaskSection(int idSection)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/section/{idSection}");
        if (response == null)
        {
            return null;
        }
        var taskProgress = await response.Content.ReadFromJsonAsync<List<TaskProgressRead>>(restClient._options);
        return new List<TaskProgress>(taskProgress.Select(dto =>
        {
            return new TaskProgress
            {
                Id = dto.Id,
                IdSection = dto.IdSection,
                Title = dto.Title,
                ModifiesProgress = dto.ModifiesProgress,
                ProgressValue = dto.ProgressValue,
                Order = dto.Order
            };
        }).ToList());
    }

    public async Task<TaskProgress> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        if (response == null)
        {
            return null;
        }
        var taskProgress = await response.Content.ReadFromJsonAsync<TaskProgressRead>(restClient._options);
        return new TaskProgress
        {
            Id = taskProgress.Id,
            IdSection = taskProgress.IdSection,
            Title = taskProgress.Title,
            ModifiesProgress = taskProgress.ModifiesProgress,
            ProgressValue = taskProgress.ProgressValue,
            Order = taskProgress.Order
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

    public async Task<TaskProgress> Post(object data)
    {
        HttpResponseMessage response = await restClient.PostAsync(route, data);
        if (response == null)
        {
            return null;
        }
        var result = await response.Content.ReadAsStringAsync();
        var taskProgress = await response.Content.ReadFromJsonAsync<TaskProgressRead>(restClient._options);
        return new TaskProgress
        {
            Id = taskProgress.Id,
            IdSection = taskProgress.IdSection,
            Title = taskProgress.Title,
            ModifiesProgress = taskProgress.ModifiesProgress,
            ProgressValue = taskProgress.ProgressValue,
            Order = taskProgress.Order
        };
    }
}
