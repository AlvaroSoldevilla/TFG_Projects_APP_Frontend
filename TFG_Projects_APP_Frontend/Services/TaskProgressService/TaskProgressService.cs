using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TaskProgressService;

public class TaskProgressService(RestClient restClient) : ITaskProgressService
{
    private readonly string route = "task_progress";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<TaskProgress>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var taskProgress = await response.Content.ReadFromJsonAsync<ObservableCollection<TaskProgress>>(restClient._options);
        return taskProgress;
    }

    public async Task<TaskProgress> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var taskProgress = await response.Content.ReadFromJsonAsync<TaskProgress>(restClient._options);
        return taskProgress;
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
