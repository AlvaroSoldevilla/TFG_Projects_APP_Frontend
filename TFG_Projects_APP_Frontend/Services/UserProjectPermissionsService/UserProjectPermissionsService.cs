using System.Collections.ObjectModel;
using System.Net.Http.Json;
using TFG_Projects_APP_Frontend.Entities.Models;
using TFG_Projects_APP_Frontend.Rest;

namespace TFG_Projects_APP_Frontend.Services.UserProjectPermissionsService;

public class UserProjectPermissionsService(RestClient restClient) : IUserProjectPermissionsService
{
    private readonly string route = "user_project_permissions";
    public async Task<string> Delete(int id)
    {
        HttpResponseMessage response = await restClient.DeleteAsync(route, id);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<ObservableCollection<UserProjectPermission>> GetAll()
    {
        HttpResponseMessage response = await restClient.GetAllAsync(route);
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<ObservableCollection<UserProjectPermission>>(restClient._options);
        return userProjectPermissions;
    }

    public async Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByPermission(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/permission/{id}");
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<ObservableCollection<UserProjectPermission>>(restClient._options);
        return userProjectPermissions;
    }

    public async Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByProject(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/project/{id}");
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<ObservableCollection<UserProjectPermission>>(restClient._options);
        return userProjectPermissions;
    }

    public async Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUser(int id)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{id}");
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<ObservableCollection<UserProjectPermission>>(restClient._options);
        return userProjectPermissions;
    }

    public async Task<ObservableCollection<UserProjectPermission>> getAllUserProjectPermissionsByUserAndProject(int userId, int projectId)
    {
        HttpResponseMessage response = await restClient.GetAllAsync($"{route}/user/{userId}/project/{projectId}");
        var userProjectPermissions = await response.Content.ReadFromJsonAsync<ObservableCollection<UserProjectPermission>>(restClient._options);
        return userProjectPermissions;
    }

    public async Task<UserProjectPermission> GetById(int id)
    {
        HttpResponseMessage response = await restClient.GetByIdAsync(route, id);
        var userProjectPermission = await response.Content.ReadFromJsonAsync<UserProjectPermission>(restClient._options);
        return userProjectPermission;
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
