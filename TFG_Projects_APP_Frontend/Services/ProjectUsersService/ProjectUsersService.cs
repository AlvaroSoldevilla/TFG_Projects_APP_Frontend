using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.ProjectUsersService;

public class ProjectUsersService(RestClient restClient) : IProjectUsersService
{
    private readonly string route = "project_users";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<ProjectUser>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var projectUsers = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectUser>>(restClient._options);
        return projectUsers;
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var projectUsers = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectUser>>(restClient._options);
        return projectUsers;
    }

    public async Task<ObservableCollection<ProjectUser>> GetAllProjectUsersByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        var projectUsers = await response.Content.ReadFromJsonAsync<ObservableCollection<ProjectUser>>(restClient._options);
        return projectUsers;
    }

    public async Task<ProjectUser> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var projectUser = await response.Content.ReadFromJsonAsync<ProjectUser>(restClient._options);
        return projectUser;
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
