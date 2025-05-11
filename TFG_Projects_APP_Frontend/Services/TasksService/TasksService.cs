using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.TasksService;

public class TasksService(RestClient restClient) : ITasksService
{
    private readonly string route = "tasks";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<ProjectTask>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var tasks = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectTask>>(restClient._options);
        return tasks;
    }

    public async Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskProgress(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/progress/{id}");
        var tasks = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectTask>>(restClient._options);
        return tasks;
    }

    public async Task<ObservableCollection<ProjectTask>> GetAllTasksByTaskSection(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/section/{id}");
        var tasks = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectTask>>(restClient._options);
        return tasks;
    }

    public async Task<ProjectTask> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var task = await response.Content.ReadFromJsonAsync<ProjectTask>(restClient._options);
        return task;
    }

    public async Task<AppUser> getUserAssigned(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}/user/assigned", id);
        var user = await response.Content.ReadFromJsonAsync<AppUser>(restClient._options);
        return user;
    }

    public async Task<AppUser> getUserCreated(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync($"{route}/user/created", id);
        var user = await response.Content.ReadFromJsonAsync<AppUser>(restClient._options);
        return user;
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
