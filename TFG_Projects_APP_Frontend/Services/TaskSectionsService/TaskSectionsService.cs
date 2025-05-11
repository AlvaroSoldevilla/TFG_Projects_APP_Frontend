using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskSectionsService;

public class TaskSectionsService(RestClient restClient) : ITaskSectionsService
{
    private readonly string route = "task_sections";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<TaskSection>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var taskSections = await response.Content.ReadFromJsonAsync<ObservableCollection<TaskSection>>(restClient._options);
        return taskSections;
    }

    public async Task<ObservableCollection<TaskSection>> getAllTaskSectionsByTaskBoard(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/board/{id}");
        var taskSections = await response.Content.ReadFromJsonAsync<ObservableCollection<TaskSection>>(restClient._options);
        return taskSections;
    }

    public async Task<TaskSection> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var taskSection = await response.Content.ReadFromJsonAsync<TaskSection>(restClient._options);
        return taskSection;
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
